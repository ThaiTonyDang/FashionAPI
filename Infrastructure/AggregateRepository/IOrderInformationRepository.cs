using Infrastructure.AggregateModel;

namespace Infrastructure.AggregateRepository
{
    public interface IOrderInformationRepository
    {
        public Task<List<OrderAggregateInformation>> GetListOrderAsync();
        public Task<List<OrderAggregateDetailInformation>> GetListOrderDetailAsync();
    }
}