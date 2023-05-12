using Domain.DTO;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Utilities.GlobalHelpers;

namespace Domain.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;
		public ProductService(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<List<ProductDto>> GetListProductsAsync()
		{
			var products = await _productRepository.GetListProducts();
			var listProducts = products.Select(p => new ProductDto
			{
				Id = p.Id,
				Name = p.Name,
				Price = p.Price.ToString(),
				Provider = p.Provider,
				Description = p.Description,
				ImageName = p.ImageName,
				QuantityInStock = p.QuantityInStock,
				IsEnabled = p.IsEnabled,
				CategoryId = p.CategoryId
			}).ToList();

			return listProducts;
		}

		public async Task<bool> AddProductAsync(ProductDto productDto)
		{
			if (productDto == null || productDto?.Price == null)
				return false;

			productDto.Id = Guid.NewGuid();
			var product = new Product()
			{
				Id = productDto.Id,
				Name = productDto.Name,
				Provider = productDto.Provider,
				Price = productDto.Price.ConvertToNumber(),
				Description = productDto.Description,
				CategoryId = productDto.CategoryId,
				ImageName = productDto.ImageName,
				QuantityInStock = productDto.QuantityInStock,
				IsEnabled = productDto.IsEnabled
			};

			var isSuccses = await _productRepository.AddAsync(product);
			return isSuccses;
		}

		public async Task<bool> UpdateProductAsync(ProductDto productDto)
		{

			var imagePath = productDto.ImageName;

			if (!string.IsNullOrEmpty(productDto.ImageName))
			{
				imagePath = productDto.ImageName;
			}

			var product = new Product
			{
				Id = productDto.Id,
				Name = productDto.Name,
				Price = productDto.Price.ConvertToNumber(),
				Provider = productDto.Provider,
				CategoryId = productDto.CategoryId,
				Description = productDto.Description,
				QuantityInStock= productDto.QuantityInStock,
				IsEnabled = productDto.IsEnabled,
				ImageName = imagePath
			};

			var result = await _productRepository.UpdateAsync(product);        
			return result;
		}

		public async Task<bool> DeleteProductAsync(Guid id)
		{
			if (id != default(Guid))
			{
				var result = await _productRepository.DeleteAsync(id);
				return result;
			}

			return false;
		}

		public async Task<ProductDto> GetProductDtoByIdAsync(Guid id)
		{
			var product = await _productRepository.GetProductByIdAsync(id);
			if (product != null)
			{
				var productItem = new ProductDto
				{
					Id = product.Id,
					Name = product.Name,
					Price = product.Price.ToString(),
					Provider = product.Provider,
					CategoryId = product.CategoryId,
					Description = product.Description,
					QuantityInStock = product.QuantityInStock,
					IsEnabled = product.IsEnabled,
					ImageName = product.ImageName
				};

				return productItem;
			}

			return null;
		}
	}
}