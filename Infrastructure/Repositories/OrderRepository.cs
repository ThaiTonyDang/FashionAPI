using Infrastructure.AggregateModel;
using Infrastructure.DataContext;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderException : Exception
    {
        public OrderException(string message) : base(message) { }
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _appDbContext;
        public OrderRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Tuple<bool, string>> CreateOrderAsync(Order order)
        {
            try
            {
                if (order == null)
                {
                    throw new OrderException("Order can not be null");
                }

                var result = await CreateOrdersAsync(order);

                return Tuple.Create(result , "Created Order Success !");

            }
            catch (Exception exception)
            {
                return Tuple.Create(false, $"An Error Occurred : {exception.Message}");
            }
        }

        public async Task<List<OrderAggregate>> GettAggregatedOrderListAsync()
        {
            var orderDetails = await _appDbContext.OrderDetails.ToListAsync();
            var baseInformations = await GetBasicInformation();

            var orders = orderDetails.Join(baseInformations, o => o.OrderId, b => b.OrderId,
                                     (o, b) => new { o.OrderId, o.Discount, b.OrderDate, b.CustomerName, b.IsPaid, b.TotalPrice, b.ShipAddress })
                                     .GroupBy(x => new { x.OrderId, x.OrderDate, x.IsPaid, x.TotalPrice, x.Discount, x.ShipAddress, x.CustomerName })
                                     .Select(x => new OrderAggregate
                                     {
                                         BaseInformation = new BaseModel
                                         {
                                             OrderId = x.Key.OrderId,
                                             OrderDate = x.Key.OrderDate,
                                             CustomerName = x.Key.CustomerName,
                                             IsPaid = x.Key.IsPaid,
                                             TotalPrice = x.Key.TotalPrice
                                         },
                                         Discount = x.Key.Discount,
                                         OrderProductsQuantity = x.Count()
                                     }).ToList();
            return orders;
        }

        public async Task<List<OrderDetailAggregate>> GetAggregatedOrderDetailAsync()
        {
            var products = await _appDbContext.Products.ToListAsync();
            var orderDetails = await _appDbContext.OrderDetails.ToListAsync();
            var baseInformations = await GetBasicInformation();
            var orderDetailAggregates = baseInformations.Join(orderDetails, b => b.OrderId, o => o.OrderId,
                             (baseInffor, orderDetail) => new
                             {
                                 baseInffor,
                                 orderDetail
                             })
                           .Join(products, x => x.orderDetail.ProductId, p => p.Id, (x, p) => new
                           {
                               product = p,
                               x.baseInffor,
                               x.orderDetail
                           }).Select(x => new OrderDetailAggregate
                           {
                               BaseInformation = x.baseInffor,
                               Product = x.product,
                               QuantityInOrder = x.orderDetail.Quantity
                           }).ToList();
            return orderDetailAggregates;
        }

        private async Task<List<BaseModel>> GetBasicInformation()
        {
            var users = await _appDbContext.Users.ToListAsync();
            var orders = await _appDbContext.Orders.ToListAsync();
            var baseInformations = users.Join(orders, user =>
                                   user.Id, order => order.UserId, (user, order)
                                   => new BaseModel
                                   {
                                       OrderId = order.Id,
                                       CustomerName = user.LastName + " " + user.FirstName,
                                       Phone = user.PhoneNumber,
                                       Email = user.Email,
                                       OrderDate = order.OrderDate,
                                       TotalPrice = order.TotalPrice,
                                       ShipAddress = user.Address,
                                       IsPaid = order.IsPaid
                                   }).ToList();

            return baseInformations;
        }

        public async Task<bool> UpdateOrderPaidStatusAsync(Guid orderId)
        {
            var orderEntity = await _appDbContext.Orders.FindAsync(orderId);
            if(orderEntity == null)
            {
                return false;
            }

            orderEntity.IsPaid = true;
            _appDbContext.Update(orderEntity);
            var result = _appDbContext.SaveChanges();
            return result > 0;
        }

        private async Task<bool> CreateOrdersAsync(Order order)
        {
            var orderdetails = order.OrderDetails.ToList();
            await _appDbContext.Orders.AddAsync(order);
            var result = await _appDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }

            return false;
        }
    }
}
