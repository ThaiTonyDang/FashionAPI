using Infrastructure.AggregateModel;
using Infrastructure.DataContext;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AggregateRepository
{
    public class OrderAggregateRepository : IOrderAggregateRepository
    {
        private readonly AppDbContext _appDbContext;
        public OrderAggregateRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<OrderAggregate>> GetListOrderAsync()
        {
            var orderDetails = await _appDbContext.OrderDetails.ToListAsync();
            var orderBaseInformation = await GetBasicInformation();
            var orderList = (from o in orderBaseInformation
                             join ods in orderDetails on o.OrderId equals ods.OrderId
                             group new { o, ods } by new
                             {
                                 o.OrderId,
                                 o.CustomerName,
                                 o.Phone,
                                 o.Email,
                                 o.OrderDate,
                                 o.TotalPrice,
                                 o.ShipAddress,
                                 ods.Discount,
                                 o.IsPaid
                             } into gr
                             let baseInformation = new BaseInformation
                             {
                                 OrderId = gr.Key.OrderId,
                                 CustomerName = gr.Key.CustomerName,
                                 Phone = gr.Key.Phone,
                                 Email = gr.Key.Email,
                                 OrderDate = gr.Key.OrderDate,
                                 TotalPrice = gr.Key.TotalPrice,
                                 ShipAddress = gr.Key.ShipAddress,
                                 IsPaid = gr.Key.IsPaid,
                             }
                             select new OrderAggregate
                             {
                                 BaseInformation = baseInformation,
                                 Discount = gr.Key.Discount,
                                 OrderProductsQuantity = gr.Count()
                             }).ToList(); return orderList;
        }

        public async Task<List<OrderDetailAggregate>> GetListOrderDetailAsync()
        {
            var products = await _appDbContext.Products.ToListAsync();
            var orderDetails = await _appDbContext.OrderDetails.ToListAsync();
            var orderBaseInformation = await GetBasicInformation();
            var orderDetailList = (from @base in orderBaseInformation
                                   join ods in orderDetails on @base.OrderId equals ods.OrderId
                                   join p in products on ods.ProductId equals p.Id
                                   let quantityInOrder = ods.Quantity
                                   let productInfo = new Product()
                                   {
                                       Id = p.Id,
                                       Name = p.Name,
                                       Price = p.Price,
                                       Provider = p.Provider,
                                       MainImageName = p.MainImageName,
                                       Description = p.Description, 
                                       CategoryId = p.CategoryId,
                                       CreatedDate = p.CreatedDate,
                                       QuantityInStock = p.QuantityInStock
                                   }
                                   let baseInformation = new BaseInformation
                                   {
                                       OrderId = @base.OrderId,
                                       CustomerName = @base.CustomerName,
                                       Phone = @base.Phone,
                                       Email = @base.Email,
                                       OrderDate = @base.OrderDate,
                                       TotalPrice = @base.TotalPrice,
                                       ShipAddress = @base.ShipAddress,
                                       IsPaid = @base.IsPaid
                                   }
                                   select new OrderDetailAggregate
                                   {
                                       BaseInformation = baseInformation,
                                       Product = productInfo,
                                       QuantityInOrder = quantityInOrder
                                   }).ToList(); return orderDetailList;
        }

        private async Task<IEnumerable<BaseInformation>> GetBasicInformation()
        {
            var users = await _appDbContext.Users.ToListAsync();
            var orders = await _appDbContext.Orders.ToListAsync();
            var orderBaseInformation = from u in users
                                       select new BaseInformation
                                       {
                                           //OrderId = o.Id,
                                           //CustomerName = c.Name,
                                           //Phone = c.Phone,
                                           //Email = c.Email,
                                           //OrderDate = o.OrderDate,
                                           //TotalPrice = o.TotalPrice,
                                           //ShipAddress = o.ShipAddress,
                                           //IsPaid = o.IsPaid
                                       }; return orderBaseInformation;
        }
    }
}