using Domain.DTO;

namespace Domain.Services
{
    public interface IProductService
	{
		public Task<bool> AddProductAsync(ProductDto product);
		//public Task<List<ProductDto>> GetListProductsAsync();
		//public Task<ProductDto> GetProductViewModel();
		//public Task<bool> EditProductAsync(ProductDto productItemViewModel);
		//public Task<bool> DeleteProductAsync(Guid id);
		//public Task<ProductDto> GetProductItemByIdAsync(Guid id);
	}
}
