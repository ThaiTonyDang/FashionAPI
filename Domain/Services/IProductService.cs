using Domain.DTO;

namespace Domain.Services
{
	public interface IProductService
	{
		public Task<bool> AddProductAsync(ProductDto product);
	}
}
