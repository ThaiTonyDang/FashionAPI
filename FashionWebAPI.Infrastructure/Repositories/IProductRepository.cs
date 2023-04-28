using FashionWebAPI.Infrastructure.Models;

namespace FashionWebAPI.Infrastructure.Repositories
{
    public interface IProductRepository
    {
        public Task<bool> AddAsync(Product product);
        public Task<List<Product>> Products();
        public Task<Product> GetProductByIdAsync(Guid id);
        public Task<bool> EditAsync(Product product);
        public Task<bool> DeleteAsync(Guid id);
    }
}
