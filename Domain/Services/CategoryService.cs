using Domain.ViewModels;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryDto>> GetListCategoryAsync()
        {
            var listCategory = await _categoryRepository.Categories();
            var listCategoryViewModel = listCategory.Select(category => new CategoryDto()
            {
                Description = category.Description,
                CategoryId = category.Id,
                Name = category.Name,
                ImagePath = category.ImagePath,
            }).ToList();

            return listCategoryViewModel;
        }
    }
}
