using Domain.Dtos;
using Infrastructure.Dtos;

namespace Domain.Services
{
    public interface IProductService
    {
        public Task<ResultDto> CreateProductAsync(ProductDto product);
        public Task<ResultDto> UpdateProductAsync(Guid productId, ProductDto productDto);
        public Task<ResultDto> DeleteProductAsync(Guid productId);
        public Task<ResultDto> GetProductByIdAsync(Guid productId);
        public Task<List<ProductDto>> GetProductsAsync(int currentPage, int pageSize);
        public Task<int> GetCountAsync();
    }
}
