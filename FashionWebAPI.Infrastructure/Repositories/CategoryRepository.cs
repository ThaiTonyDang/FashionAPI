using FashionWebAPI.Infrastructure.DataContext;
using FashionWebAPI.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace FashionWebAPI.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;
        public CategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Category>> Categories()
        {
            var categories = await _appDbContext.Categories.ToListAsync();
            return categories;
        }
    }
}
