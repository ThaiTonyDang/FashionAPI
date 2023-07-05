using Domain.Dtos;

namespace Domain.Services
{
    public interface ICategoryService
    {
        public Task<Tuple<bool, string>> CreateCategoryAsync(CategoryDto categoryDto);
        public Task<Tuple<bool, string>> UpdateCategoryAsync(CategoryDto categoryDto);
        public Task<Tuple<bool, string>> DeleteCategoryAsync(Guid id);
        public Task<Tuple<CategoryDto, string>> GetCategoryByIdAsync(Guid id);
        public Task<List<CategoryDto>> GetCategoryListAsync();
        public Task<int> GetTotalCategory();

    }
}
