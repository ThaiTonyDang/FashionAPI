using Domain.DTO;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface ICategoryService
    {
        public Task<List<CategoryDto>> GetListCategoryAsync();
        public Task<Tuple<bool, string>> CreateCategoryAsync(CategoryDto categoryDto);
        public Task<Tuple<bool, string>> UpdateCategoryAsync(CategoryDto categoryDto);
        public Task<Tuple<bool, string>> DeleteCategoryAsync(Guid id);
        public Task<Tuple<CategoryDto, string>> GetCategoryById(Guid id);
    }
}
