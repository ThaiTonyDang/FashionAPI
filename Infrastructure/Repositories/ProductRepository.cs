using Infrastructure.DataContext;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductException : Exception
    {
        public ProductException(string message) : base(message){}
    }

    public class ProductRepository : IProductRepository        
    {
        private readonly AppDbContext _appDbContext;
        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AddAsync(Product product)
        {
            if(product == null)
            {
               throw new ProductException("Product can not be null");
            }

            if (product.Id == default(Guid) || product.CategoryId == default(Guid))
                return false;

            var productEntity = _appDbContext.Products
                                             .Where(p => p.Name == product.Name && p.Provider == product.Provider)
                                             .FirstOrDefault();

            if (productEntity == null)
            {
                await _appDbContext.AddAsync(product);
                var result = await _appDbContext.SaveChangesAsync();
                return (result > 0);
            }

            return false;
        }

        public async Task<List<Product>> GetListProducts()
        {
            var list = await _appDbContext.Products.ToListAsync();
            return list;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            if (product == null)
            {
                throw new ProductException("Product can not be null");
            }

            if (product.Id == default(Guid) || product.CategoryId == default(Guid))
                return false;

            var productEntity = await GetProductByIdAsync(product.Id);

            if (productEntity != null)
            {
                productEntity.Id = product.Id;
                productEntity.Name = product.Name;
                productEntity.Provider = product.Provider;
                productEntity.Price = product.Price;
                productEntity.Description = product.Description;
                productEntity.CategoryId = product.CategoryId;
                productEntity.ImagePath = product.ImagePath;
                productEntity.QuantityInStock = product.QuantityInStock;
                productEntity.IsEnabled = product.IsEnabled;

                _appDbContext.Update(productEntity);
                var result = _appDbContext.SaveChanges();
                return (result > 0);
            }

            return false;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await GetProductByIdAsync(id);

            if (product != null)
            {
                _appDbContext.Remove(product);
                var result = await _appDbContext.SaveChangesAsync();
                return (result > 0);
            }

            return false;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            var product = await _appDbContext.Products.Where(p => p.Id == id).FirstOrDefaultAsync();

            return product;
        }
    }
}
