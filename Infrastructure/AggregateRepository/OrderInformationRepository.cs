using Infrastructure.AggregateModel;
using Infrastructure.DataContext;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AggregateRepository
{
    public class OrderInformationRepository : IOrderInformationRepository
    {
        private readonly AppDbContext _appDbContext;
        public OrderInformationRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<OrderAggregateInformation>> GetListOrderAsync()
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
                             }
                             select new OrderAggregateInformation
                             {
                                 BaseInformation = baseInformation,
                                 Discount = gr.Key.Discount,
                                 OrderProductsQuantity = gr.Count()
                             }).ToList(); return orderList;
        }

        public async Task<List<OrderAggregateDetailInformation>> GetListOrderDetailAsync()
        {
            var products = await _appDbContext.Products.ToListAsync();
            var orderDetails = await _appDbContext.OrderDetails.ToListAsync();
            var orderBaseInformation = await GetBasicInformation();
            var orderDetailList = (from @base in orderBaseInformation
                                   join ods in orderDetails on @base.OrderId equals ods.OrderId
                                   join p in products on ods.ProductId equals p.Id
                                   let quantity = ods.Quantity
                                   let productInfo = new Product()
                                   {
                                       Id = p.Id,
                                       Name = p.Name,
                                       Price = p.Price,
                                       Provider = p.Provider,
                                       ImageName = p.ImageName
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
                                   }
                                   select new OrderAggregateDetailInformation
                                   {
                                       BaseInformation = baseInformation,
                                       Product = productInfo,
                                       Quantity = quantity
                                   }).ToList(); return orderDetailList;
        }

        private async Task<IEnumerable<BaseInformation>> GetBasicInformation()
        {
            var customers = await _appDbContext.Customers.ToListAsync();
            var orders = await _appDbContext.Orders.ToListAsync();
            var orderBaseInformation = from c in customers
                                       join o in orders on c.Id equals o.CustomerId
                                       select new BaseInformation
                                       {
                                           OrderId = o.Id,
                                           CustomerName = c.Name,
                                           Phone = c.Phone,
                                           Email = c.Email,
                                           OrderDate = o.OrderDate,
                                           TotalPrice = o.TotalPrice,
                                           ShipAddress = o.ShipAddress,
                                       }; return orderBaseInformation;
        }
    }
}