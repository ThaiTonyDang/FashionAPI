 using Infrastructure.DataContext;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryException : Exception
    {
        public CategoryException(string message) : base(message) { }
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;
        public CategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Tuple<bool, string>> CreateAsync(Category category)
        {
            if (category == null)
            {
                throw new CategoryException("Category can not be null");
            }

            if (category.Id == default(Guid)) return Tuple.Create(false, "Category Id Is Not Default");

            var categoryEntity = _appDbContext.Categories
                                                .Where(c => c.Name.ToLower() == category.Name.ToLower() && 
                                                c.ParentCategoryId == category.ParentCategoryId)
                                                .FirstOrDefault();
            if (categoryEntity != null) return Tuple.Create(false, "Category already exists");

            await _appDbContext.AddAsync(category);
            var result = await _appDbContext.SaveChangesAsync();
            return Tuple.Create(true, "Created Successful !");
        }

        public async Task<Tuple<bool, string>> UpdateAsync(Category category)
        {
            
            if (category == null)
            {
                throw new CategoryException("Category Can Not Be Null");
            }

            if (category.Id == default(Guid))
                return Tuple.Create(false, "Category Id Invalid !");
            var categoryEntity = _appDbContext.Categories.Where(c => c.Id != category.Id && c.Name == category.Name &&
                                                                c.ParentCategoryId == category.ParentCategoryId)
                                                                .FirstOrDefault();
            if (categoryEntity != null) return Tuple.Create(false, "Category Name Already Exists! Can't Update");

            categoryEntity = await GetCategoryById(category.Id);
            if (categoryEntity == null || categoryEntity.Id == default(Guid))
            {
                return Tuple.Create(false, "Category Not Found! Update Failed !");
            }

            categoryEntity.Name = category.Name;
            categoryEntity.Description = category.Description;
            categoryEntity.ImageName = category.ImageName;
            categoryEntity.CreatedDate = category.CreatedDate;
            categoryEntity.ModifiedDate = category.ModifiedDate;
            categoryEntity.ParentCategoryId = category.ParentCategoryId;

            _appDbContext.Update(categoryEntity);
            var result = _appDbContext.SaveChanges();
            var message = "Update Category Success !";
            return Tuple.Create(result > 0, message);
            
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var categoryEntity = await _appDbContext.Categories
                                       .Include(c => c.CategoryChildren)
                                       .FirstOrDefaultAsync(c => c.Id == id);
            if (categoryEntity == null)
            {
                return false;
            }

            foreach (var child in categoryEntity.CategoryChildren)
            {
                child.ParentCategoryId = categoryEntity.ParentCategoryId;
            }

            _appDbContext.Categories.Remove(categoryEntity);
            var result = _appDbContext.SaveChanges();
            return (result > 0);                             
        }

        public async Task<Category> GetCategoryById(Guid id)
        {
            var categoryEntity = await _appDbContext.Categories
                                       .Include(c => c.CategoryChildren)
                                       .Include(c => c.ParentCategory)
                                       .FirstOrDefaultAsync(c => c.Id == id);
            if (categoryEntity == null)
            {
                return null;
            }

            return categoryEntity;          
        }

        public async Task<List<Category>> GetCategoryListAsync()
        {
            var qr = (from c in _appDbContext.Categories select c)
                      .Include(c => c.Products)
                      .Include(c => c.ParentCategory)
                      .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync()).Where(c => c.ParentCategory == null).ToList();
            return categories;
        }

        public Task<int> GetTotal()
        {
            return _appDbContext.Categories.CountAsync();
        }

    }
}
