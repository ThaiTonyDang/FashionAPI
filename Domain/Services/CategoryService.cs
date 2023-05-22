using Domain.Dtos;
using Infrastructure.Models;
using Infrastructure.Repositories;

namespace Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Tuple<bool, string>> CreateCategoryAsync(CategoryDto categoryDto)
        {
            if (categoryDto == null)
                return Tuple.Create(false, "The Category To Be Created Cannot Found");

            categoryDto.Id = Guid.NewGuid();
            var category = new Category()
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name,
                Description = categoryDto.Description             
            };

            var result = await _categoryRepository.CreateAsync(category);
            return result;
        }

        public async Task<List<CategoryDto>> GetListCategoryAsync()
        {
            var listCategory = await _categoryRepository.GetListCategoryAsync();
            var listCategoryViewModel = listCategory.Select(category => new CategoryDto()
            {
                Description = category.Description,
                Id = category.Id,
                Name = category.Name,
                ImageName = category.ImageName,
            }).ToList();

            return listCategoryViewModel;
        }

        public async Task<Tuple<bool, string>> UpdateCategoryAsync(CategoryDto categoryDto)
        {
            if (categoryDto == null)
                return Tuple.Create(false, "The Category To Be Created Cannot Found");
            var category = new Category
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            var result = await _categoryRepository.UpdateAsync(category);
            var isSuccess = result.Item1;
            var message = result.Item2;

            return Tuple.Create(isSuccess, message);
        }


        public async Task<Tuple<bool, string>> DeleteCategoryAsync(Guid id)
        {
            if (id == default(Guid))
            {
                return Tuple.Create(false, "Id Invalid ! Delete Fail!");
            }    
            var result = await _categoryRepository.DeleteAsync(id);
            var isSuccess = result.Item1;
            var message = result.Item2;

            return Tuple.Create(isSuccess, message);
        }

        public async Task<Tuple<CategoryDto, string>> GetCategoryById(Guid id)
        {
            if (id == default(Guid))
            {
                return Tuple.Create(default(CategoryDto), "Id Invalid ! Cannot Get Category !");
            }
            var result = await _categoryRepository.GetCategoryById(id);
            var category = result.Item1;
            var message = result.Item2;

            if (category == null)
            {
                return Tuple.Create(default(CategoryDto), "Cannot Found Category !");
            }

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return Tuple.Create(categoryDto, message);
        }
    }
}
