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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<Tuple<bool, string>> CreateOrder(OrderDto orderDto)
        {
            if (orderDto == null)
                return Tuple.Create(false, "The Order To Be Created Doesn't Exist");

            var order = new Order()
            {
                Id = orderDto.Id,
                OrderDate = orderDto.OrderDate,
                RequiredDate = orderDto.RequiredDate,
                ShipAddress = orderDto.ShipAddress,
                Status = orderDto.Status,
                IsPaid = orderDto.IsPaid,
                TotalPrice = orderDto.TotalPrice,
                //CustomerId = orderDto.CustomerId,             
            };

            var result = await _orderRepository.CreateOrder(order);
            return result;
        }
    }
}
