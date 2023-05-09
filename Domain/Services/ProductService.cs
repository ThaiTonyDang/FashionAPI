﻿using Domain.DTO;
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
				Price = p.Price,
				Provider = p.Provider,
				Description = p.Description,
				ImagePath = p.ImagePath,
				QuantityInStock = p.QuantityInStock,
				IsEnabled = p.IsEnabled,
				CategoryId = p.CategoryId
			}).ToList();

			return listProducts;
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

		public async Task<bool> UpdateProductAsync(ProductDto productDto)
		{
			
			var imagePath = DISPLAY.DEFAULT_IMAGE;

			if (productDto.ImagePath != null)
			{
				imagePath = productDto.ImagePath;
			}

			var product = new Product
			{
				Id = productDto.Id,
				Name = productDto.Name,
				Price = productDto.Price,
				Provider = productDto.Provider,
				CategoryId = productDto.CategoryId,
				Description = productDto.Description,
				QuantityInStock= productDto.QuantityInStock,
				IsEnabled = productDto.IsEnabled,
				ImagePath = imagePath
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
					CategoryId = product.CategoryId,
				};

				return productItem;
			}

			return null;
		}
	}
}
