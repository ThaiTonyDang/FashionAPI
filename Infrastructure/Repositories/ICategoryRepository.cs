using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface ICategoryRepository
    {
        public Task<Tuple<bool, string>> CreateAsync(Category category);
        public Task<Tuple<bool, string>> UpdateAsync(Category category);
        public Task<bool> DeleteAsync(Guid id);
        public Task<Category> GetCategoryById(Guid id);
        public Task<List<Category>> GetCategoryListAsync();
        public Task<int> GetTotal();
    }
}
