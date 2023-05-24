using Domain.DTO;
using Infrastructure.Models;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        public OrderDetailService(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }
        public async Task<Tuple<bool, string>> CreateOrderDetail(OrderDetailDto orderDetailDto)
        {
            var orderDetail = new OrderDetail()
            {
              OrderId = orderDetailDto.OrderId,
              ProductId = orderDetailDto.ProductId,
              Price = orderDetailDto.Price,
              Quantity = orderDetailDto.Quantity,
              Discount = orderDetailDto.Discount
            };

            var result = await _orderDetailRepository.CreateOrderDetail(orderDetail);
            return result;
        }
    }
}
