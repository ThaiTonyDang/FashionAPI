using Infrastructure.DataContext;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly AppDbContext _appDbContext;
        public OrderDetailRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Tuple<bool, string>> CreateOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                if (orderDetail == null)
                {
                    throw new OrderException("Order detail can not be null");
                }

                await _appDbContext.OrderDetails.AddAsync(orderDetail);
                var result = await _appDbContext.SaveChangesAsync();
                return Tuple.Create(result > 0, "Created Order Detail Success !");

            }
            catch (Exception exception)
            {
                return Tuple.Create(false, $"An Error Occurred : {exception.Message}");
            }
        }
    }
}
