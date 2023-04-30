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

		//public async Task<ProductDto> GetProductViewModel()
		//{
		//	var productViewModel = new ProductDto();
		//	productViewModel.ListProduct = await GetListProductsAsync();
		//	return productViewModel;
		//}

		//public async Task<List<ProductDto>> GetListProductsAsync()
		//{
		//	var products = await _productRepository.GetListProducts();
		//	var listProducts = products.Select(p => new ProductIItemDto
		//	{
		//		Id = p.Id,
		//		Name = p.Name,
		//		Price = p.Price,
		//		Provider = p.Provider,
		//		Description = p.Description,
		//		ImagePath = p.ImagePath,
		//		UnitsInStock = p.UnitsInStock,
		//		Enable = p.Enable,
		//		Type = p.Type,
		//		CategoryId = p.CategoryId
		//	}).ToList();

		//	return listProducts;
		//}

		//public async Task<bool> EditProductAsync(ProductDto productItemViewModel)
		//{
		//	try
		//	{
		//		var imagePath = DISPLAY.DEFAULT_IMAGE_PATH;

		//		if (productItemViewModel.ImagePath != null)
		//		{
		//			imagePath = productItemViewModel.ImagePath;
		//		}    

		//		if (productItemViewModel.Image != null )
		//		{
		//			var fileName = productItemViewModel.Image.FileName;
		//			imagePath = _fileService.GetFilePath(fileName);
		//		}

		//		var product = new Product
		//		{
		//			Id = productItemViewModel.Id,
		//			Name = productItemViewModel.Name,
		//			Price = productItemViewModel.Price,
		//			Provider = productItemViewModel.Provider,
		//			CategoryId = productItemViewModel.CategoryId,
		//			Description = productItemViewModel.Description,
		//			UnitsInStock = productItemViewModel.UnitsInStock,
		//			Type = productItemViewModel.Type,
		//			Enable = productItemViewModel.GetEnable(),
		//			ImagePath = imagePath
		//		};

		//		var result = await _productRepository.EditAsync(product);
		//		if (result)
		//		{
		//			if (productItemViewModel.Image != null)
		//			{                     
		//				var data = await productItemViewModel.Image.GetBytes();
		//				var folderExtra = nameof(Product);
		//				await this._fileService.SaveFile(folderExtra, imagePath, data);           
		//			}

		//			return true;
		//		}
		//	}
		//	catch (Exception)
		//	{
		//		return false;
		//	}

		//	return false;
		//}

		//public async Task<ProductDto> GetProductItemByIdAsync(Guid id)
		//{
		//	var product = await _productRepository.GetProductByIdAsync(id);
		//	// đặt điều kiện
		//	if (product != null)
		//	{
		//		var productItem = new ProductIItemDto
		//		{
		//			Id = product.Id,
		//			Name = product.Name,
		//			Price = product.Price,
		//			Provider = product.Provider,
		//			CategoryId = product.CategoryId,
		//			Description = product.Description,
		//			UnitsInStock = product.UnitsInStock,
		//			Type = product.Type,
		//			Enable = product.Enable,
		//			ImagePath = product.ImagePath
		//		};

		//		return productItem;
		//	}

		//	return null;
		//}

		//public async Task<bool> DeleteProductAsync(Guid id)
		//{
		//	if (id != default(Guid))
		//	{
		//		var result = await _productRepository.DeleteAsync(id);
		//		return result;
		//	}

		//	return false;
		//}
	}
}
