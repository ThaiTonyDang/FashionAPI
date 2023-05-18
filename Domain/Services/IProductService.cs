using Domain.DTO;

namespace Domain.Services
{
    public interface IProductService
    {
        public Task<Tuple<bool, string>> CreateProductAsync(ProductDto product);
        public Task<List<ProductDto>> GetListProductsAsync();
        public Task<Tuple<bool, string>> UpdateProductAsync(ProductDto productDto);
        public Task<Tuple<bool, string>> DeleteProductAsync(Guid id);
        public Task<Tuple<ProductDto, string>> GetProductDtoByIdAsync(Guid id);
    }
}
