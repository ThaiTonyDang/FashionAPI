using Domain.AggregateModelDto;
using Domain.DTO;
using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IOrderService
    {
        public Task<Tuple<bool, string>> CreateOrderAsync(OrderDto orderItemViewModel);
        public Task<Tuple<bool, string>> CreateOrderDetailAsync(OrderDetailDto orderDetailDto);
        public Task<List<OrderAggregateDto>> GetAggregatedOrderListAsync();
        public Task<List<OrderDetailAggregateDto>> GetAggregatedOrderDetailByIdAsync(Guid orderId);
        public Task<Tuple<BaseInformationDto, string>> GetOrderedBaseInformationAsync(Guid orderId);
        public Task<Tuple<List<ProductDto>, string>> GetOrderedProductListAsync(Guid orderId);
        public Task<List<OrderDetailAggregateDto>> GetAggregatedOrderDetailListAsync();

    }
}
