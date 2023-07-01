using Domain.Dtos;
using Infrastructure.DataContext;
using Infrastructure.Dtos;
using Infrastructure.Models;
using Infrastructure.Repositories;
using System.Xml.Linq;

namespace Domain.Services
{
    public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<ResultDto> CreateProductAsync(ProductDto productDto)
		{
			if (productDto == null || productDto?.Price == null)
				return new ErrorResult("The Product to be created doesn't exist or price value is invalid");

			var product = new Product()
			{
				Id = Guid.NewGuid(),
				Name = productDto.Name,
                Price = productDto.Price,
                Provider = productDto.Provider,
                MainImageName = productDto.MainImageName,
                IsEnabled = productDto.IsEnabled,
                CategoryId = productDto.CategoryId,
                Description = productDto.Description,
				CreatedDate = DateTime.UtcNow,
				ModifiedDate = DateTime.UtcNow,
				QuantityInStock = productDto.QuantityInStock,
			};

			var result = await _productRepository.CreateAsync(product);
			return result;
		}

		public async Task<ResultDto> UpdateProductAsync(Guid productId, ProductDto productDto)
		{
			var existProduct = await _productRepository.CheckExitDuplicateProduct(productId,
																				productDto.Name,
																				productDto.Provider);
            if (existProduct)
            {
                return new ErrorResult("Product name with provider is already exists");
            }

            var getProductResult = await _productRepository.GetProductByIdAsync(productId);
            if (!getProductResult.IsSuccess)
            {
                return getProductResult;
            }

            var product = getProductResult.ToSuccessDataResult<Product>().Data;

			product.Name = productDto.Name;
			product.Price = productDto.Price;
			product.Provider = productDto.Provider;
			product.MainImageName = productDto.MainImageName;
			product.IsEnabled = productDto.IsEnabled;
			product.CategoryId = productDto.CategoryId;
			product.Description = productDto.Description;
			product.ModifiedDate = DateTime.UtcNow;
			product.QuantityInStock = productDto.QuantityInStock;

			var result = await _productRepository.UpdateAsync(product);
			return result;
		}

		public async Task<ResultDto> DeleteProductAsync(Guid productId)
		{
            var getProductResult = await _productRepository.GetProductByIdAsync(productId);
            if (!getProductResult.IsSuccess)
            {
                return new ErrorResult("Delete product failed cause product is not found");
            }

            var product = getProductResult.ToSuccessDataResult<Product>().Data;

            var result = await _productRepository.DeleteAsync(product);
            return result;
        }

        public async Task<ResultDto> GetProductByIdAsync(Guid productId)
		{
            var result = await _productRepository.GetProductByIdAsync(productId);

			if(!result.IsSuccess)
			{
				return result;
			}
            
			var product = result.ToSuccessDataResult<Product>().Data;
			var productDto = new ProductDto
			{
				Id = product.Id,
				Name = product.Name,
				Price = product.Price,
				Provider = product.Provider,
				CategoryId = product.CategoryId,
				Description = product.Description,
				QuantityInStock = product.QuantityInStock,
				IsEnabled = product.IsEnabled,
				MainImageName = product.MainImageName,
				ModifiedDate = product.ModifiedDate,
				CreateDate = product.CreatedDate
			};

			return new SuccessDataResult<ProductDto>(result.Message, productDto);
        }

        public async Task<List<ProductDto>> GetProductsAsync(int currentPage, int pageSize)
        {
            var products = await _productRepository.GetProductsAsync(currentPage, pageSize);
            var listProducts = products.Select(product => new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Provider = product.Provider,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,
                MainImageName = product.MainImageName,
				QuantityInStock = product.QuantityInStock,
				CreateDate = product.CreatedDate,
            }).ToList();

            return listProducts;
        }

        public async Task<int> GetCountAsync()
        {
			return await _productRepository.GetCountAsync();
        }
    }
}