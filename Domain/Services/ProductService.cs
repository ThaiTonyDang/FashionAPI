using Domain.Dtos;
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

		public async Task<List<ProductDto>> GetListProductsAsync()
		{
			var products = await _productRepository.GetListProductsAsync();
			var listProducts = products.Select(p => new ProductDto
			{
				Id = p.Id,
				Name = p.Name,
				Price = p.Price,
				Provider = p.Provider,
				Description = p.Description,
				ImageName = p.ImageName,
				QuantityInStock = p.QuantityInStock,
				IsEnabled = p.IsEnabled,
				CategoryId = p.CategoryId
			}).ToList();

			return listProducts;
		}

		public async Task<Tuple<bool, string>> CreateProductAsync(ProductDto productDto)
		{
			if (productDto == null || productDto?.Price == null)
				return Tuple.Create(false, "The Product To Be Created Doesn't Exist Or Price Value Is Invalid");

			productDto.Id = Guid.NewGuid();
			var product = new Product()
			{
				Id = productDto.Id,
				Name = productDto.Name,
				Provider = productDto.Provider,
				Price = productDto.Price,
				Description = productDto.Description,
				CategoryId = productDto.CategoryId,
				ImageName = productDto.ImageName,
				QuantityInStock = productDto.QuantityInStock,
				IsEnabled = productDto.IsEnabled
			};

			var result = await _productRepository.CreateAsync(product);
			return result;
		}

		public async Task<Tuple<bool, string>> UpdateProductAsync(ProductDto productDto)
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
				Price = productDto.Price,
				Provider = productDto.Provider,
				CategoryId = productDto.CategoryId,
				Description = productDto.Description,
				QuantityInStock= productDto.QuantityInStock,
				IsEnabled = productDto.IsEnabled,
				ImageName = imagePath
			};

			var result = await _productRepository.UpdateAsync(product);
            var isSuccess = result.Item1;
            var message = result.Item2;

            return Tuple.Create(isSuccess, message);
		}

		public async Task<Tuple<bool, string>> DeleteProductAsync(Guid id)
		{
            if (id == default)
            {
                return Tuple.Create(false, "Id Invalid ! Delete Fail!");
            }
            var result = await _productRepository.DeleteAsync(id);
            var isSuccess = result.Item1;
            var message = result.Item2;

            return Tuple.Create(isSuccess, message);
        }

        public async Task<Tuple<ProductDto, string>> GetProductDtoByIdAsync(Guid id)
		{
            if (id == default)
            {
                return Tuple.Create(default(ProductDto), "Id Invalid ! Cannot Get Prodcut !");
            }

            var result = await _productRepository.GetProductByIdAsync(id);
            var product = result.Item1;
            var message = result.Item2;
            if (product == null)
			{ 
                return Tuple.Create(default(ProductDto), $"{message}");
            }
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
				ImageName = product.ImageName
			};

		    return Tuple.Create(productDto, message);

        }
    }
}