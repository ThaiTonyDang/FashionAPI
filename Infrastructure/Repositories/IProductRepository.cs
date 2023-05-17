using Infrastructure.Models;

namespace Infrastructure.Repositories
{
    public interface IProductRepository
    {
        public Task<Tuple<bool, string>> CreateAsync(Product product);
        public Task<List<Product>> GetListProductsAsync();
        public Task<Product> GetProductByIdAsync(Guid id);
        public Task<bool> UpdateAsync(Product product);
        public Task<bool> DeleteAsync(Guid id);
    }
}