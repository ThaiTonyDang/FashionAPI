using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IOrderService
    {
        public Task<Tuple<bool, string>> CreateOrder(OrderDto orderItemViewModel);
    }
}
