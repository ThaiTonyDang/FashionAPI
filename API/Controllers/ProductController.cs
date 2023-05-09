using Domain.DTO;
using Domain.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDto product)
        {
            if (product != null)
            {
                var result = await _productService.AddProductAsync(product);

                if (result)
                    return Ok(new
                    {
                        Success = true,
                        Data = product
                    });
            }    
            
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetListProducts()
        {
            var products = await _productService.GetListProductsAsync();

            return Ok(new
            {
                Success = true,
                Data = products
            });
        }

        [HttpPut]
        [Route("{productId}")]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            var productEntity = await _productService.GetProductDtoByIdAsync(product.Id);
            if (productEntity == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Product not found"
                });
            }

            var isSuccess = await _productService.UpdateProductAsync(productEntity);

            if (isSuccess)
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Update Success",
                    Success = true,
                    Data = productEntity
                });
            }

            return BadRequest(new
            {
                StatusCode = 400,
                Success = false,
                Message = "Update Fail !"
            });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _productService.GetProductDtoByIdAsync(id);
            if (product == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Product Not Found !"
                });
            }

            var isSuccess = await _productService.DeleteProductAsync(id);

            if (isSuccess)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Delete Success!",
                    Data = product
                });
            }

            return BadRequest(new
            {
                StatusCode = 400,
                Success = false,
                Message = "Delete Fail !"
            });
        }
    }
}
