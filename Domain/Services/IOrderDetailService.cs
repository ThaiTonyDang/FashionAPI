using Domain.DTO;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IOrderDetailService
    {
        public Task<Tuple<bool, string>> CreateOrderDetail(OrderDetailDto orderDetailDto);
    }
}
