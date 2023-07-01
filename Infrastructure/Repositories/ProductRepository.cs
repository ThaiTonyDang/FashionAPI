using Infrastructure.DataContext;
using Infrastructure.Dtos;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;

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

        public async Task<ResultDto> CreateAsync(Product product)
        {
            if (product == null)
                throw new ProductException("Product can not be null");

            if (product.Id == default || product.CategoryId == default)
            {
                return new ErrorResult("Product and category id is invalid");
            }

            var productEntity = _appDbContext.Products
                                             .Where(p => 
                                                  p.Name.ToLower() == product.Name.ToLower() &&
                                                  p.Provider.ToLower() == product.Provider.ToLower()
                                              ).FirstOrDefault();

            if (productEntity != null)
                return new ErrorResult("Product name and product provider already exist!");

            await _appDbContext.Products.AddAsync(product);
            var result = await _appDbContext.SaveChangesAsync();

            if(result < 0)
            {
                return new ErrorResult("Create new product failed!");
            }

            return new SuccessResult("Created product successfuly");
        }

        public async Task<List<Product>> GetProductListAsync()
        {
            var list = await _appDbContext.Products.ToListAsync();
            return list;
        }

        public async Task<ResultDto> UpdateAsync(Product product)
        {
            _appDbContext.Products.Update(product);
            var result = await _appDbContext.SaveChangesAsync();
            if(result == 0)
            {
                return new ErrorResult("No product record is updated");
            }

            return new SuccessResult("Update product successfully");
        }

        public Task<bool> CheckExitDuplicateProduct(Guid productId, string productName, string provider)
        {
            var existProduct = _appDbContext.Products
                                             .Where(p => p.Id != productId
                                                && p.Name == productName
                                                && p.Provider == provider)
                                             .FirstOrDefault();
            return Task.FromResult(existProduct != null);
        }

        public async Task<ResultDto> DeleteAsync(Product product)
        {
            _appDbContext.Products.Remove(product);
            var result = await _appDbContext.SaveChangesAsync();
            if (result == 0)
            {
                return new ErrorResult("No product is deleted");
            }

            return new SuccessResult("Delete product successfully");
        }

        public async Task<ResultDto> GetProductByIdAsync(Guid productId)
        {
            var product = await _appDbContext.Products.FindAsync(productId);

            if (product == null) 
            {
                return new ErrorResult("Product is not found!");
            } 

            return new SuccessDataResult<Product>("Product is founded", product);
        }

        public async Task<List<Product>> GetProductsAsync(int currentPage, int pageSize)
        {
            var products = await _appDbContext.Products.AsNoTracking()
                                                       .AsQueryable()
                                                       .OrderBy(p => p.Name)
                                                       .Skip((currentPage - 1) * pageSize)
                                                       .Take(pageSize)
                                                       .ToListAsync();
            return products;
        }

        public async Task<int> GetCountAsync()
        {
            return await _appDbContext.Products.CountAsync();
        }
    }
}