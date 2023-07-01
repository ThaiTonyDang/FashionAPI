using Infrastructure.Dtos;
using Infrastructure.Models;

namespace Infrastructure.Repositories
{
    public interface IProductRepository
    {
        Task<ResultDto> CreateAsync(Product product);
        Task<List<Product>> GetProductListAsync();
        Task<List<Product>> GetProductsAsync(int currentPage, int pageSize);
        Task<ResultDto> UpdateAsync(Product product);
        Task<bool> CheckExitDuplicateProduct(Guid productId, string productName, string provider);
        Task<ResultDto> DeleteAsync(Product product);
        Task<ResultDto> GetProductByIdAsync(Guid productId);
        Task<int> GetCountAsync();
    }
}