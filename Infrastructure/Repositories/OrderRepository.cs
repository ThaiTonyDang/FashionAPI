using Infrastructure.DataContext;
using Infrastructure.Models;
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
        public async Task<Tuple<bool, string>> CreateOrder(Order order)
        {
            try
            {
                if (order == null)
                {
                    throw new OrderException("Order can not be null");
                }

                if (order.CustomerId == default(Guid))
                    return Tuple.Create(false, "Customer Id Can Not Be Null");

                await _appDbContext.Orders.AddAsync(order);
                var result = await _appDbContext.SaveChangesAsync();
                return Tuple.Create(result > 0, "Created Order Success !");

            }
            catch (Exception exception)
            {
                return Tuple.Create(false, $"An Error Occurred : {exception.Message}");
            }
        }
    }
}
