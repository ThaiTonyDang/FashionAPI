using Domain.Dtos;
using Domain.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Utilities.GlobalHelpers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDto product)
        {
            var message = "";
            if (product == null)
            {
                return NotFound(new
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                    Message = "Product Cannot Be Empty !"
                });
            }

            var result = await _productService.CreateProductAsync(product);
            var isSuccess = result.Item1;
            message = result.Item2;
            if (isSuccess)
            return Ok(new
            {
                StatusCode = HttpStatusCode.Created,
                Success = true,
                Message = $"{message}"
            });

            return BadRequest(new
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Message = $"{message}",
            });         
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var products = await _productService.GetListProductsAsync();

            return Ok(new
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Message = "Get list success",
                Data = products,
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            var result = await _productService.UpdateProductAsync(productDto);
            var isSuccess = result.Item1;
            var message = result.Item2;
            if (isSuccess)
            {
                return Ok(new
                {
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Message = $"{message}",                    
                });
            }
            return BadRequest(new
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Message = $"{message}"
            });
        }

        [HttpDelete]
        [Route("{productId}")]
        public async Task<IActionResult> Delete(string productId)
        {
            if (!productId.IsConvertToGuid())
            {
                return BadRequest(new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Product Id Is InValid ! Delete Failed !"
                });
            }    
            var result = await _productService.DeleteProductAsync(new Guid(productId));
            var isSuccess = result.Item1;
            var message = result.Item2;
            if (isSuccess)
            {
                return Ok(new
                {
                    StatusCode = HttpStatusCode.NoContent,
                    Success = true,
                    Message = $"{message}",
                });
            }
            return BadRequest(new
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Message = $"{message}"
            });                            
        }

        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> GetProduct(string productId)
        {
            if (!productId.IsConvertToGuid())
            {
                return BadRequest(new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Product Id Is InValid ! Delete Failed !"
                });
            }
            var result = await _productService.GetProductDtoByIdAsync(new Guid(productId));
            var productDto = result.Item1;
            var message = result.Item2;
            if (productDto != null)
            {
                return Ok(new
                {
                    StatusCode = HttpStatusCode.NoContent,
                    Success = true,
                    Message = $"{message}",
                    Data = productDto
                });
            }
            return BadRequest(new
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Message = $"{message}"
            });

        }
    }
}
