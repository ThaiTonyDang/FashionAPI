using Domain.Dtos;
using Infrastructure.Models;
using Infrastructure.Repositories;
using SixLabors.ImageSharp;
using System.Xml.Linq;
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
                CreatedDate = categoryDto.CreateDate,
                ParentCategoryId = categoryDto.ParentCategoryId
            };

            var result = await _categoryRepository.CreateAsync(category);
            return result;
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
                ModifiedDate = categoryDto.ModifiedDate,
                ParentCategoryId = categoryDto.ParentCategoryId
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
            var isSuccess = await _categoryRepository.DeleteAsync(id);
            if (isSuccess) return Tuple.Create(true, "Deleted successful !");
            return Tuple.Create(isSuccess, "Deleted fail !");
        }

        public async Task<Tuple<CategoryDto, string>> GetCategoryByIdAsync(Guid id)
        {
            if (id == default(Guid))
            {
                return Tuple.Create(default(CategoryDto), "Id Invalid ! Cannot Get Category !");
            }
            var category = await _categoryRepository.GetCategoryById(id);

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
                ModifiedDate = category.ModifiedDate,
                ParentCategoryId = category.ParentCategoryId

            };

            return Tuple.Create(categoryDto, "Get Category Successful !");
        }

        public async Task<List<CategoryDto>> GetCategoryListAsync()
        {
            var categories = await _categoryRepository.GetCategoryListAsync();

            var categoryDtos = new List<CategoryDto>();
            MapDataCategory(categoryDtos, categories);

            int i = 0;
            foreach (var category in categories)
            {
                if(categoryDtos[i].CategoryChildrens == null)
                {
                    var destination = new List<CategoryDto>();
                    var sourceList = category.CategoryChildren.ToList();
                    MapDataCategory(destination, sourceList);
                    categoryDtos[i].CategoryChildrens = destination;
                }                               
                i++;
            }

            return categoryDtos;
        }

        private void MapDataCategory(List<CategoryDto> des, List<Category> source)
        {

            foreach (var category in source)
            {
                var categoryDto = new CategoryDto();
                categoryDto.Id = category.Id;
                categoryDto.Name = category.Name;
                categoryDto.Description = category.Description;
                categoryDto.ImageName = category.ImageName;
                categoryDto.CreateDate = category.CreatedDate;
                categoryDto.ParentCategoryId = category.ParentCategoryId;
                categoryDto.Slug = category.Slug;
                if(categoryDto.ProductDtos == null)
                {
                    var productDtos = new List<ProductDto>();
                    MapDataProduct(productDtos, category.Products.ToList());
                    categoryDto.ProductDtos = productDtos;
                }    
              
                des.Add(categoryDto);
            }
        }


        private void MapDataProduct(List<ProductDto> productDtos, List<Product> products)
        {
            foreach (var product in products)
            {
                var productDto = new ProductDto();
                productDto.Id = product.Id;
                productDto.Name = product.Name;
                productDto.MainImageName = product.MainImageName;
                productDto.Provider = product.Provider;
                productDto.CategoryId = product.CategoryId;
                productDto.Description = product.Description;
                productDto.CreateDate = product.CreatedDate;
                productDto.QuantityInStock = product.QuantityInStock;
                productDto.Price = product.Price;
                productDtos.Add(productDto);
            }             
        }

        public Task<int> GetTotalCategory()
        {
            return _categoryRepository.GetTotal();
        }
    }
}