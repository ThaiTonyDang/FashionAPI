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
    public class OrderInfoemationRepository : IOrderInformationRepository
    {
        private readonly AppDbContext _appDbContext;
        public OrderInfoemationRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<OrderInformation>> GetListOrder()
        {
            var customers = await _appDbContext.Customers.ToListAsync();
            var orders = await _appDbContext.Orders.ToListAsync();
            var orderDetails = await _appDbContext.OrderDetails.ToListAsync();
            var products = await _appDbContext.Products.ToListAsync();
            var orderBaseInformation = from c in customers
                                       join o in orders on c.Id equals o.CustomerId
                                       select new
                                       {
                                           o.Id,
                                           CustomerId = c.Id,
                                           c.Name,
                                           c.Phone,
                                           c.Email,
                                           o.OrderDate,
                                           o.TotalPrice,
                                           o.ShipAddress,
                                           o.IsPaid
                                       };

            var orderList = from order in orderBaseInformation
                            join orderDetail in orderDetails on order.Id equals orderDetail.OrderId
                            group new { order, orderDetail } by new
                            {
                                order.Id,
                                CustomerId = order.Id,
                                Name = order.Name,
                                CustomerPhone = order.Phone,
                                CustomerEmail = order.Email,
                                Date = order.OrderDate,
                                Total = order.TotalPrice,
                                Address = order.ShipAddress,
                                order.IsPaid,
                            } into gr



            return orderList;
        }
    }
}
