using Domain.Dtos;
using Infrastructure.Dtos;
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

		public async Task<List<ProductDto>> GetProductListAsync()
		{
			var products = await _productRepository.GetProductListAsync();
			var listProducts = products.Select(p => new ProductDto
			{
				Id = p.Id,
				Name = p.Name,
				Price = p.Price,
				Provider = p.Provider,
                MainImageName = p.MainImageName,
                IsEnabled = p.IsEnabled,
                CategoryId = p.CategoryId,
                Description = p.Description,
				QuantityInStock = p.QuantityInStock,
			}).ToList();

			return listProducts;
		}

		public async Task<Result> CreateProductAsync(ProductDto productDto)
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
				QuantityInStock = productDto.QuantityInStock,
			};

			var result = await _productRepository.CreateAsync(product);
			return result;
		}

		public async Task<Tuple<bool, string>> UpdateProductAsync(ProductDto productDto)
		{
			var imagePath = productDto.MainImageName;
			if (!string.IsNullOrEmpty(productDto.MainImageName))
			{
				imagePath = productDto.MainImageName;
			}
			var product = new Product
			{
				Id = productDto.Id,
				Name = productDto.Name,
				Price = productDto.Price,
				Provider = productDto.Provider,
                MainImageName = imagePath,
                IsEnabled = productDto.IsEnabled,
                CategoryId = productDto.CategoryId,
                Description = productDto.Description,
				ModifiedDate = productDto.ModifiedDate,
				QuantityInStock= productDto.QuantityInStock,		
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
				MainImageName = product.MainImageName,
				ModifiedDate = product.ModifiedDate,
				CreateDate = product.CreatedDate
			};

		    return Tuple.Create(productDto, message);

        }

        public async Task<List<ProductDto>> GetPagingProductListAsync(int currentPage, int pageSize)
        {
            var listProduct = await _productRepository.GetPagingProductListAsync(currentPage, pageSize);
            var listProductViewModel = listProduct.Select(product => new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Provider = product.Provider,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,
				CategoryName = product.Category.Name,
                MainImageName = product.MainImageName,
				QuantityInStock = product.QuantityInStock,
				CreateDate = product.CreatedDate,
            }).ToList();

            return listProductViewModel;
        }

        public async Task<int> GetTotalItems()
        {
			return await _productRepository.GetTotalItems();
        }
    }
}