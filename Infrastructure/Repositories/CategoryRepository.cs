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
            try
            {
                if (category == null)
                {
                    throw new CategoryException("Category can not be null");
                }

                if (category.Id == default(Guid)) return Tuple.Create(false, "Category Id Is Invalid");
                var categoryEntity = _appDbContext.Categories
                                                  .Where(c => c.Name == category.Name)
                                                  .FirstOrDefault();
                if (categoryEntity != null) return Tuple.Create(false, "Category Name Already Exists");
                await _appDbContext.AddAsync(category);
                var result = await _appDbContext.SaveChangesAsync();
                return Tuple.Create(result > 0, "Created Category Success !");
            }
            catch (Exception exception)
            {
                return Tuple.Create(false, $"An Error Occurred : {exception.InnerException.Message}");
            }
        }

        public async Task<List<Category>> GetListCategoryAsync()
        {
            var categories = await _appDbContext.Categories.ToListAsync();
            return categories;
        }

        public async Task<Tuple<bool, string>> UpdateAsync(Category category)
        {
            try
            {
                if (category == null)
                {
                    throw new CategoryException("Category Can Not Be Null");
                }

                if (category.Id == default(Guid))
                    return Tuple.Create(false, "Category Id Invalid !");
                var categoryEntity = _appDbContext.Categories.Where(c => c.Id != category.Id && c.Name == category.Name)
                                                             .FirstOrDefault();
                if (categoryEntity != null) return Tuple.Create(false, "Category Name Already Exists! Can't Update");

                var searchResult = await GetCategoryById(category.Id);
                categoryEntity = searchResult.Item1;
                var message = searchResult.Item2;
                if (categoryEntity == null || categoryEntity.Id == default(Guid))
                {
                    return Tuple.Create(false, message + " Update Failed !");
                }

                categoryEntity.Name = category.Name;
                categoryEntity.Description = category.Description;

                _appDbContext.Update(categoryEntity);
                var result = _appDbContext.SaveChanges();
                message = "Edit Category Success !";
                return Tuple.Create(result > 0, message);
            }
            catch (Exception exception)
            {
                return Tuple.Create(false, $"An Error Occurred : {exception.InnerException.Message}");
            }
        }

        public async Task<Tuple<bool, string>> DeleteAsync(Guid id)
        {
            try
            {
                var searchResult = await GetCategoryById(id);

                var categoryEntity = searchResult.Item1;
                var message = searchResult.Item2;

                if (categoryEntity == null)
                {
                    return Tuple.Create(false, message + " Deleted Category Failed !");
                }
                _appDbContext.Categories.Remove(categoryEntity);
                var result = _appDbContext.SaveChanges();
                return Tuple.Create(result > 0, "Deleled Category Success !");
            }
            catch (Exception exception)
            {
                return Tuple.Create(false, $"An Error Occurred : {exception.InnerException.Message}! Deleted Category Failed !");
            }                   
        }

        public async Task<Tuple<Category, string>> GetCategoryById(Guid id)
        {
            try
            {
                var category = await _appDbContext.Categories.FindAsync(id);

                if (category == null) return Tuple.Create(default(Category), "Category Not Found !");
                return Tuple.Create(category, "Found the Success Category");
            }
            catch (Exception exception)
            {
                return Tuple.Create(default(Category), $"{exception.InnerException.Message} ");
            }
        }
    }
}
