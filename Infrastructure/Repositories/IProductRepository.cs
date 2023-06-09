using Infrastructure.Models;

namespace Infrastructure.Repositories
{
    public interface IProductRepository
    {
        public Task<Tuple<bool, string>> CreateAsync(Product product);
        public Task<List<Product>> GetListProductsAsync();
        public Task<Tuple<bool, string>> UpdateAsync(Product product);
        public Task<Tuple<bool, string>> DeleteAsync(Guid id);
        public Task<Tuple<Product, string>> GetProductByIdAsync(Guid id);
    }
}