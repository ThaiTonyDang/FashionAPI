﻿using Infrastructure.DataContext;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Test.RepositoriesTest
{
	public class ProductRepositoryTest
	{
		private readonly AppDbContext _dbContext;
		private readonly ProductRepository _productRepository;
		public ProductRepositoryTest()
		{
			var appContextOptions = new DbContextOptionsBuilder<AppDbContext>()
								   .UseInMemoryDatabase(databaseName: $"Ecommercy_{DateTime.Now.ToFileTimeUtc()}")
								   .Options;

			_dbContext = new AppDbContext(appContextOptions);
			_productRepository = new ProductRepository(_dbContext);

			SeddingDataAsync(_dbContext)
							.ConfigureAwait(true)
							.GetAwaiter()
							.GetResult();
		}

		[Fact]
		public async Task Should_GetListProduct_Return_Success()
		{
			// Arrage
			var productListExpect = LoadProducSampletData();

			// Act
			var productListAct = await _productRepository.GetListProducts();

			// Assert
			Assert.Equal(productListExpect.Count, productListAct.Count);

			var firtItemExpect = productListExpect.FirstOrDefault();
			var firtItemAct = productListAct.FirstOrDefault();

			Assert.Equal(firtItemExpect!.Name, firtItemAct!.Name);
		}

		[Fact]
		public async Task Should_AddProductsAsync_Return_Success()
		{
			// Arrage
			var productExpect = new Product()
			{
				Id = Guid.NewGuid(),
				Name = "ABC",
				Provider = "Fc",
				CategoryId = LoadCategorySampleData()[1].Id,
			};

			var countExpext = 4;

			// Act
			var resultAct = await _productRepository.AddAsync(productExpect);

			// Assert        
			Assert.True(resultAct);
			Assert.Equal(countExpext, _dbContext.Products.ToList().Count());
		}

		[Fact]
		public async Task Should_AddProductsAsync_Return_Fail_When_Id_Default()
		{
			// Arrage
			var productExpect = new Product()
			{
				Id = default(Guid),
				Name = "ABC",
				Provider = "Fc",
				CategoryId = LoadCategorySampleData()[2].Id,
			};

			var countExpext = 3;

			// Act
			var resultAct = await _productRepository.AddAsync(productExpect);

			// Assert        
			Assert.False(resultAct);
			Assert.Equal(countExpext, _dbContext.Products.ToList().Count());
		}

		[Fact]
		public async Task Should_AddProductsAsync_Return_Fail_When_Name_And_Provider_Is_Exits()
		{
			// Arrage
			var productExpect = new Product()
			{
				Id = Guid.NewGuid(),
				Name = "Đồng Hồ",
				Provider = "Chelsea Fc",
				CategoryId = LoadCategorySampleData()[2].Id,
			};

			var countExpext = 3;

			// Act
			var resultAct = await _productRepository.AddAsync(productExpect);

			// Assert        
			Assert.False(resultAct);
			Assert.Equal(countExpext, _dbContext.Products.ToList().Count());
		}

		[Fact]
		public async Task Should_EditProductAsync_Return_Succsess()
		{
			// Arrange
			var product = LoadProducSampletData()[0];

			// Act
			var name = "ABC";
			var enable = true;
			product.Name = name;
			product.IsEnabled = enable;

			var isEditSuccess = await _productRepository.UpdateAsync(product);

			// Assert
			Assert.True(isEditSuccess);
			Assert.Equal(name, product.Name);
			Assert.Equal(enable, product.IsEnabled);
		}

		[Fact]
		public async Task Should_EditProductAsync_Return_Fail_When_Id_Null()
		{
			// Arrange
			var product = LoadProducSampletData()[0];
			product.Id = default(Guid);

			// Act
			var name = "ABC";
			var enable = true;
			product.Name = name;
			product.IsEnabled = enable;

			var isEditSuccess = await _productRepository.UpdateAsync(product);

			// Assert
			Assert.False(isEditSuccess);
		}

		[Fact]
		public async Task Should_DeleteProductAsync_Return_Success()
		{
			// Arrange
			var productId = LoadProducSampletData()[0].Id;
			var COUNT_EXPECT = 2;

			// Act
			var isDelete = await _productRepository.DeleteAsync(productId);
			var listProductAct = await _productRepository.GetListProducts();
			var productAct = listProductAct.Where(p => p.Id == productId).FirstOrDefault();

			// Assert
			Assert.True(isDelete);
			Assert.Equal(COUNT_EXPECT, listProductAct.Count);
			Assert.Null(productAct);
		}

		[Fact]
		public async Task Should_DeleteProductAsync_Return_Fail_When_Id_Not_Exists()
		{
			// Arrange
			var productId = Guid.NewGuid();
			var COUNT_EXPECT = 3;

			// Act
			var isDelete = await _productRepository.DeleteAsync(productId);
			var listProductAct = await _productRepository.GetListProducts();

			// Assert
			Assert.False(isDelete);
			Assert.Equal(COUNT_EXPECT, listProductAct.Count);
		}

		private List<Product> LoadProducSampletData()
		{
			var listCates = LoadCategorySampleData();

			return new List<Product>()
			{
				new Product()
				{
					Id = Guid.Parse("561232ff-efc8-4580-b681-4ec31beb79b8"),
					Name = "Đồng Hồ",
					Provider = "Chelsea Fc",
					Price = 10,
					CategoryId = listCates[0].Id,
					Description = "Đồng Hồ Chelsea Chính Hãng",
					ImageName = "Pepsi20230216034120.jpg",
					IsEnabled = false
				},

				new Product()
				{
					Id = Guid.Parse("2c4b115e-ad9a-4259-8732-1f23e019802b"),
					Name = "Áo Choàng Mùa Đông Chelsea",
					Provider = "Fashion VN",
					Price = 100,
					CategoryId = listCates[1].Id,
					Description = "Áo Choàng Thời Trang",
					ImageName = "pho-bo20230216053008.jpg",
					IsEnabled = true
				},

				new Product()
				{
					Id = Guid.Parse("f4a55c69-46db-499b-998e-2bdeb01166e0"),
					Name = "Mũ Lưỡi Trai",
					Provider = "Valve",
					Price = 10,
					CategoryId = listCates[2].Id,
					Description = "Mũ",
					ImageName = "pho-bo20230216053008.jpg",
					IsEnabled = true
				}
			};
		}

		private async Task SeddingDataAsync(AppDbContext appDbContext)
		{
			var categories = LoadCategorySampleData();
			var products = LoadProducSampletData();

			await appDbContext.Categories.AddRangeAsync(categories);
			await appDbContext.Products.AddRangeAsync(products);

			await appDbContext.SaveChangesAsync();
		}

		private List<Category> LoadCategorySampleData()
		{   
			return new List<Category>()
			{
				new Category
				{
					Id = Guid.NewGuid(),
					Name = "Trang Sức",
					Description = "Trang Sức"
				},

				new Category
				{
					Id = Guid.NewGuid(),
					Name = "Áo Choàng",
					Description = "Food Type"
				},

				new Category
				{
					Id = Guid.NewGuid(),
					Name = "Mũ",
					Description = "Entertainment Type"
				}
			};
		}
	}
}