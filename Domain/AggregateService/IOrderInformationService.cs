using Domain.AggregateModelDto;
using Domain.Dtos;

namespace Domain.AggregateService
{
    public interface IOrderInformationService
    {
        public Task<List<OrderInforDto>> GetListOrderInformationAsync();
        public Task<Tuple<SingleOrderDetailDto, string>> GetSingleOrderDetailByIdAsync(Guid orderId);
    }
}