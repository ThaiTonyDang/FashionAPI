using Infrastructure.AggregateModel;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IOrderRepository
    {
        public Task<Tuple<bool, string>> CreateOrderAsync(Order order);
        public Task<Tuple<bool, string>> CreateOrderDetailAsync(OrderDetail orderDetail);
        public Task<List<OrderAggregate>> GettAggregatedOrderListAsync();
        public Task<List<OrderDetailAggregate>> GetAggregatedOrderDetailAsync();
    }
}
