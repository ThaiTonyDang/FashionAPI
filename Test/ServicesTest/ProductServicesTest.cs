using Domain.DTO;
using Domain.Services;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Moq;

namespace Test.ServicesTest
{
    public class ProductServicesTest
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        public ProductServicesTest()
        {
            _productRepoMock = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task Should_AddProductAsync_Return_Success()
        {
            // Arrange
            var productViewModel = new ProductDto()
            {
                Id = Guid.NewGuid(),
                Price = 100,
                CategoryId = Guid.Parse("46a023f4-549b-4f45-8242-a94a2f5e5eb8"),
            };

            _productRepoMock.Setup(m => m.AddAsync(It.IsAny<Product>())).ReturnsAsync(true);

            // Act
            var productService = new ProductService(_productRepoMock.Object);
            var isSuccess = await productService.AddProductAsync(productViewModel);

            // Assert
            Assert.True(isSuccess);
        }

        [Fact]
        public async Task Should_AddProductAsync_Return_Fail_When_Id_Default()
        {
            // Arrange
            var productViewModel = new ProductDto()
            {
                Id = default(Guid),
                Price = 100,
            };

            _productRepoMock.Setup(m => m.AddAsync(It.IsAny<Product>())).ReturnsAsync(false);

            // Act
            var productService = new ProductService(_productRepoMock.Object);
            var isSuccess = await productService.AddProductAsync(productViewModel);

            // Assert
            Assert.False(isSuccess);
        }

        [Fact]
        public async Task Should_AddProductAsync_Return_Error_When_ProductDto_Null()
        {
            // Arrange
            var productViewModel = default(ProductDto);

            _productRepoMock.Setup(m => m.AddAsync(It.IsAny<Product>())).ReturnsAsync(true);

            // Act
            var productService = new ProductService(_productRepoMock.Object);
            var isSuccess = await productService.AddProductAsync(productViewModel);

            // Assert
            Assert.False(isSuccess);
        }

        [Fact]
        public async Task Should_GetListProductAsync_Return_Success()
        {
            // Arrage
            var productExpect = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Quần Kaki",
                CategoryId = Guid.NewGuid()
            };

            var productsExpect = new List<Product>() { productExpect };
            _productRepoMock.Setup(mbox => mbox.GetListProducts()).ReturnsAsync(productsExpect);

            // Act
            var productService = new ProductService(_productRepoMock.Object);
            var productsAct = await productService.GetListProductsAsync();

            // Assert
            Assert.Equal(productsExpect.Count, productsAct.Count);

            var productFirtItemAct = productsAct.FirstOrDefault();

            Assert.NotNull(productFirtItemAct!.Name);
            Assert.Equal(productExpect.Name, productFirtItemAct!.Name);
            Assert.Equal(productExpect.Id, productFirtItemAct!.Id);
        }

        [Fact]
        public async Task Should_EditProductAsync_Return_Success()
        {
            // Arrange
            var productViewModel = new ProductDto()
            {
                Id = new Guid(),
                Name = "ABC"
            };

            _productRepoMock.Setup(m => m.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(true);

            // Act
            var productService = new ProductService(_productRepoMock.Object);
            var isSuccess = await productService.UpdateProductAsync(productViewModel);

            // Assert
            Assert.True(isSuccess);
        }

        [Fact]
        public async Task Should_EditProductAsync_Return_Fail_When_Id_Default()
        {
            // Arrange
            var productViewModel = new ProductDto()
            {
                Id = default(Guid)
            };

            _productRepoMock.Setup(m => m.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(false);

            // Act
            var productService = new ProductService(_productRepoMock.Object);
            var isSuccess = await productService.UpdateProductAsync(productViewModel);

            // Assert
            Assert.False(isSuccess);
        }

        [Fact]
        public async Task Should_EditProductAsync_Return_Fail_When_Id_Not_Found()
        {
            // Arrange
            var productViewModel = new ProductDto()
            {
                Id = Guid.NewGuid()
            };

            _productRepoMock.Setup(m => m.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(false);

            // Act
            var productService = new ProductService(_productRepoMock.Object);
            var isSuccess = await productService.UpdateProductAsync(productViewModel);

            // Assert
            Assert.False(isSuccess);
        }

        [Fact]
        public async Task Should_DeleteProductAsync_Return_Succes()
        {
            // Arrange
            var id = Guid.NewGuid();

            _productRepoMock.Setup(m => m.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            // Act
            var productService = new ProductService(_productRepoMock.Object);
            var isSuccess = await productService.DeleteProductAsync(id);

            // Assert
            Assert.True(isSuccess);
        }

        [Fact]
        public async Task Should_DeleteProductAsync_Return_False_When_ID_Default()
        {
            // Arrange
            var id = default(Guid);

            _productRepoMock.Setup(m => m.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            // Act
            var productService = new ProductService(_productRepoMock.Object);
            var isSuccess = await productService.DeleteProductAsync(id);

            // Assert
            Assert.False(isSuccess);
        }

        [Fact]
        public async Task Should_GetProductById_Return_Product()
        {
            // Arrange
            var productExpect = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Quần Kaki",
                CategoryId = Guid.NewGuid()
            };

            _productRepoMock.Setup(m => m.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync(productExpect);

            // Act
            var productService = new ProductService(_productRepoMock.Object);
            var productDto = await productService.GetProductDtoByIdAsync(productExpect.Id);

            // Assert
            Assert.NotNull(productDto);
        }

        [Fact]
        public async Task Should_GetProductById_Return_Null_When_Id_Not_Found()
        {
            // Arrange
            var productExpect = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Quần Kaki",
                CategoryId = Guid.NewGuid()
            };

            _productRepoMock.Setup(m => m.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product?)null);

            // Act
            var productService = new ProductService(_productRepoMock.Object);
            var productDto = await productService.GetProductDtoByIdAsync(productExpect.Id);

            // Assert
            Assert.Null(productDto);
        }
    }
}