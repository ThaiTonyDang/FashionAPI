using Domain.Dtos;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{

    public class OrderInformationService : IOrderInformationService
    {
        private readonly IOrderInformationRepository _orderManagementRepository;
        public OrderInformationService(IOrderInformationRepository orderManagementRepository)
        {
            _orderManagementRepository = orderManagementRepository;
        }

        public async Task<List<OrderInforDto>> GetListOrderInfor()
        {
            var listOrder = await _orderManagementRepository.GetListOrder();
            var orders = listOrder.Select(o => new OrderInforDto
            {
                OrderId = o.OrderId,
                CustomerName = o.CustomerName,
                Phone = o.Phone,
                Email = o.Email,
                ShipAddress = o.ShipAddress,
                IsPaid =  o.IsPaid,
                OrderDate = o.OrderDate,
                OrderProductsQuantity = o.OrderProductsQuantity,
                TotalPrice = o.TotalPrice
            }).ToList();

            return orders;
        }
    }
}
