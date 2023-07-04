﻿using Infrastructure.DataContext;
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

        public async Task<Result> CreateAsync(Product product)
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

        public async Task<Tuple<bool, string>> UpdateAsync(Product product)
        {
            try
            {
                if (product == null)
                {
                    throw new ProductException("Product can not be null");
                }
                if (product.Id == default(Guid) || product.CategoryId == default(Guid))
                   return Tuple.Create(false, "Product Id Or Category Id Invalid !");
                var productEntity = _appDbContext.Products.Where(p => p.Id != product.Id 
                                                           && p.Name == product.Name && p.Provider == product.Provider)
                                                           .FirstOrDefault();
                if (productEntity != null) return Tuple.Create(false, "Product Name And Provider Already Exists! Try Again");

                var searchResult = await GetProductByIdAsync(product.Id);
                productEntity = searchResult.Item1;
                var message = searchResult.Item2;

                if (productEntity == null)
                {
                    return Tuple.Create(false , message + " Updated Product Failed !");
                }
                productEntity.Id = product.Id;
                productEntity.Name = product.Name;
                productEntity.Provider = product.Provider;
                productEntity.Price = product.Price;
                productEntity.Description = product.Description;
                productEntity.CategoryId = product.CategoryId;
                productEntity.MainImageName = product.MainImageName;
                productEntity.QuantityInStock = product.QuantityInStock;
                productEntity.IsEnabled = product.IsEnabled;
                productEntity.ModifiedDate = product.ModifiedDate;

                _appDbContext.Products.Update(productEntity);
                var result = _appDbContext.SaveChanges();
                return Tuple.Create(result > 0, "Updated Product Success !");                         
            }
            catch (Exception exception)
            {
                return Tuple.Create(false, $"An Error Occurred : {exception.Message}! Updated Product Fail !");
            }
        }

        public async Task<Tuple<bool, string>> DeleteAsync(Guid id)
        {
            try
            {
                var searchResult = await GetProductByIdAsync(id);

                var productEntity = searchResult.Item1;
                var message = searchResult.Item2;

                if (productEntity == null)
                {
                    return Tuple.Create(false, message + " Deleted Product Failed !");
                }
                _appDbContext.Products.Remove(productEntity);
                var result = _appDbContext.SaveChanges();
                return Tuple.Create(result > 0, "Deleled Product Success !");
            }
            catch (Exception exception)
            {
                return Tuple.Create(false, $"An Error Occurred : {exception.InnerException.Message}!" +
                                   $" Deleted Product Failed !");
            }
        }

        public async Task<Tuple<Product, string>> GetProductByIdAsync(Guid id)
        {
            try
            {
                var product = await _appDbContext.Products.FindAsync(id);

                if (product == null) return Tuple.Create(default(Product), "Not Found! Product Does Not Exist !");

                return Tuple.Create(product, "Found the Success Product");
            }
            catch (Exception exception)
            {
                return Tuple.Create(default(Product), $"{exception.Message}! Product Does Not Exist !");
            }

        }

        public async Task<List<Product>> GetPagingProductListAsync(int currentPage, int pageSize)
        {
            var products = await _appDbContext.Products.Include(x => x.Category)
                                                       .AsNoTracking() 
                                                       .OrderByDescending(p => p.CreatedDate)
                                                       .Skip((currentPage - 1) * pageSize)
                                                       .Take(pageSize).AsQueryable()
                                                       .ToListAsync();
            return products;
        }

        public async Task<int> GetTotalItems()
        {
            return await _appDbContext.Products.CountAsync();
        }
    }
}