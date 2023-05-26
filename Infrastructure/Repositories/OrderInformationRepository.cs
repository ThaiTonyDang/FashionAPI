using Infrastructure.DataContext;
using Infrastructure.JoinItem;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderInformationRepository : IOrderInformationRepository
    {
        private readonly AppDbContext _appDbContext;
        public OrderInformationRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<OrderInformation>> GetListOrder()
        {
            var orderDetails = await _appDbContext.OrderDetails.ToListAsync();
            var orderBaseInformation = await GetBasicInformation();
            var orderList = (from o in orderBaseInformation join ods in orderDetails on o.OrderId equals ods.OrderId
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
                             select new OrderInformation
                             {
                                 OrderId = gr.Key.OrderId,
                                 CustomerName = gr.Key.CustomerName,
                                 Phone = gr.Key.Phone,
                                 Email = gr.Key.Email,
                                 OrderDate = gr.Key.OrderDate,
                                 TotalPrice = gr.Key.TotalPrice,
                                 ShipAddress = gr.Key.ShipAddress,
                                 Discount = gr.Key.Discount,
                                 OrderProductsQuantity = gr.Count()
                             }).ToList();
            return orderList;
        }

        public async Task<List<OrderDetailInformation>> GetListOrderDetail()
        {
            var products = await _appDbContext.Products.ToListAsync();
            var orderDetails = await _appDbContext.OrderDetails.ToListAsync();
            var orderBaseInformation = await GetBasicInformation();
            var orderDetailList = (from o in orderBaseInformation
                             join ods in orderDetails on o.OrderId equals ods.OrderId
                             join p in products on ods.ProductId equals p.Id
                             select new OrderDetailInformation
                             {
                                 OrderId = o.OrderId,
                                 CustomerName = o.CustomerName,
                                 Phone = o.Phone,
                                 Email = o.Email,
                                 OrderDate = o.OrderDate,
                                 TotalPrice = o.TotalPrice,
                                 ShipAddress = o.ShipAddress,
                                 ProductName = p.Name,
                                 Provider = p.Provider,
                                 Price = p.Price,
                                 Quantity = ods.Quantity
                             }).ToList();
            return orderDetailList;
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
                                       };
            return orderBaseInformation;
        }
    }
}
