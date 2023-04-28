using Infrastructure.Models;

namespace Infrastructure.Repositories
{
    public interface IProductRepository
    {
        public Task<bool> AddAsync(Product product);
        public Task<List<Product>> GetListProducts();
        public Task<Product> GetProductByIdAsync(Guid id);
        public Task<bool> EditAsync(Product product);
        public Task<bool> DeleteAsync(Guid id);
    }
}
