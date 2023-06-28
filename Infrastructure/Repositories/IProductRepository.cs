using Infrastructure.Dtos;
using Infrastructure.Models;

namespace Infrastructure.Repositories
{
    public interface IProductRepository
    {
        public Task<Result> CreateAsync(Product product);
        public Task<List<Product>> GetProductListAsync();
        public Task<List<Product>> GetPagingProductListAsync(int currentPage, int pageSize);
        public Task<Tuple<bool, string>> UpdateAsync(Product product);
        public Task<Tuple<bool, string>> DeleteAsync(Guid id);
        public Task<Tuple<Product, string>> GetProductByIdAsync(Guid id);
        public Task<int> GetTotalItems();
    }
}