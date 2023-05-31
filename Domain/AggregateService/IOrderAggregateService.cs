using Domain.AggregateModelDto;
using Domain.Dtos;

namespace Domain.AggregateService
{
    public interface IOrderAggregateService
    {
        public Task<List<OrderAggregateDto>> GetOrderListAsync();
        public Task<List<OrderDetailAggregateDto>> GetOrderDetailByIdAsync(Guid orderId);
        public Task<Tuple<BaseInformationDto, string>> GetOrderedBaseInformationAsync(Guid orderId);
        public Task<Tuple<List<ProductDto>, string>> GetOrderedProductListAsync(Guid orderId);
        public Task<List<OrderDetailAggregateDto>> GetOrderDetailListAsync();
    }
}