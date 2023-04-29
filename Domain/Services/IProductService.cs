using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
	public interface IProductService
	{
		public Task<bool> AddProductAsync(ProductItemViewModel productItemViewModel);
		public Task<List<ProductItemViewModel>> GetListProductsAsync();
		public Task<ProductViewModel> GetProductViewModel();
		public Task<bool> UpdateProductAsync(ProductItemViewModel productItemViewModel);
		public Task<bool> DeleteProductAsync(Guid id);
		public Task<ProductItemViewModel> GetProductItemByIdAsync(Guid id);
	}
}
