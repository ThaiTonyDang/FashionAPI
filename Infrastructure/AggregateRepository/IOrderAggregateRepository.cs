using Infrastructure.AggregateModel;

namespace Infrastructure.AggregateRepository
{
    public interface IOrderAggregateRepository
    {
        public Task<List<OrderAggregate>> GetListOrderAsync();
        public Task<List<OrderDetailAggregate>> GetListOrderDetailAsync();
    }
}