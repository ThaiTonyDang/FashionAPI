using Domain.Dtos;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Utilities.GlobalHelpers;

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
                Description = categoryDto.Description,
                ImageName = categoryDto.ImageName,
                CreatedDate = categoryDto.CreateDate
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
                CreateDate = category.CreatedDate,
                ModifiedDate = category.ModifiedDate

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
                Description = categoryDto.Description,
                ImageName = categoryDto.ImageName,
                ModifiedDate = categoryDto.ModifiedDate
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

        public async Task<Tuple<CategoryDto, string>> GetCategoryByIdAsync(Guid id)
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
                Description = category.Description,
                ImageName = category.ImageName,
                CreateDate = category.CreatedDate,
                ModifiedDate = category.ModifiedDate
            };

            return Tuple.Create(categoryDto, message);
        }

        public async Task<List<ProductDto>> GetProductsByName(int categoryCode)
        {
            var categoryName = GetCategoryNameByCode(categoryCode);
            var products = await _categoryRepository.GetProductsByName(categoryName);
            if (products == null)
            {
                return null;
            }    
            var productDto = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Provider = p.Provider,
                CategoryId = p.CategoryId,
                Description = p.Description,
                QuantityInStock = p.QuantityInStock,
                MainImageName = p.MainImageName,
            }).ToList();

            return productDto;
        }

        private string GetCategoryNameByCode(int categoryCode)
        {
            switch (categoryCode)
            {
                case ((int)CATEGORY_CODE.MEN):
                    return CATEGORY.MEN_FASHION;
                case ((int)CATEGORY_CODE.WOMEN):
                    return CATEGORY.WOMEN_FASHION;
                default:
                    return CATEGORY.KID_FASHION;
            }
        }
    }
}