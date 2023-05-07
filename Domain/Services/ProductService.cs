using Domain.DTO;
using Infrastructure.Models;
using Infrastructure.Repositories;

namespace Domain.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;
		public ProductService(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<bool> AddProductAsync(ProductDto productDto)
		{
			if (productDto == null)
				return false;

			var product = new Product()
			{
				Id = productDto.Id,
				Name = productDto.Name,
				Provider = productDto.Provider,
				Price = productDto.Price,
				Description = productDto.Description,
				CategoryId = productDto.CategoryId,
				ImagePath = productDto.ImagePath,
				QuantityInStock = productDto.QuantityInStock,
				IsEnabled = productDto.IsEnabled
			};

			var isSuccses = await _productRepository.AddAsync(product);
			return isSuccses;
		}
	}
}
