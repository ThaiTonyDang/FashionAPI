using Domain.DTO;
using Domain.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Utilities.GlobalHelpers;

namespace API.Controllers
{
    [Route($"{HTTTP.VERSION_V1}/")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost($"{HTTTP.PRODUCT_ENDPOINTS}")]
        public async Task<IActionResult> Create(ProductDto product)
        {
            if (product != null)
            {
                var result = await _productService.AddProductAsync(product);

                if (result)
                    return Ok(new
                    {
                        Message = "Created !",
                        StatusCode = 201,
                        Success = true,
                    }) ;

                return BadRequest(new
                {
                    Message = "Name or Provider already exists or Something Happened",
                    StatusCode = 400,
                    Success = false,
                });
            }

            return NotFound(new
            {
                StatusCode = 404,
                Success = false,
                Message = "Product cannot be Empty !"
            });
        }

        [HttpGet($"{HTTTP.PRODUCT_ENDPOINTS}")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetListProductsAsync();

            return Ok(new
            {
                StatusCode = 200,
                Success = true,
                Data = products
            });
        }

        [HttpPut]
        [Route($"{HTTTP.PRODUCT_ENDPOINTS}/{{productId}}")]
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
        [Route($"{HTTTP.PRODUCT_ENDPOINTS}/{{productId}}")]
        public async Task<IActionResult> Delete(string productId)
        {
            if (productId.IsConvertToGuid())
            {
                var product = await _productService.GetProductDtoByIdAsync(new Guid(productId));
                if (product == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Success = false,
                        Message = "Product Not Found !"
                    });
                }

                var isSuccess = await _productService.DeleteProductAsync(new Guid(productId));

                if (isSuccess)
                {
                    return Ok(new
                    {
                        StatusCode = 204,
                        Success = true,
                        Message = "Deleted !",
                    });
                }

                return BadRequest(new
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "Delete Fail !"
                });
            }
          
            return BadRequest(new
            {
                StatusCode = 400,
                Success = false,
                Message = "Id InValid !"
            });
        }

        [HttpGet]
        [Route($"{HTTTP.PRODUCT_ENDPOINTS}/{{productId}}")]
        public async Task<IActionResult> GetSingleProduct(string productId)
        {
            if (productId.IsConvertToGuid())
            {
                var product = await _productService.GetProductDtoByIdAsync(new Guid(productId));

                if (product != null)
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Success = true,
                        Data = product
                    });
                }

                return NotFound(new
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Product Not Found !"
                });
            }

            return BadRequest(new
            {
                StatusCode = 400,
                Success = false,
                Message = "Id Invalid !"
            });
        }
    }
}
