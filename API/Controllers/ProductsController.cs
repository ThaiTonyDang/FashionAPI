using API.Dtos;
using Domain.Dtos;
using Domain.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest(
                    new Error<string>(
                        (int)HttpStatusCode.BadRequest,
                        "Create product failed",
                        "Product can not be null")
                    );
            }

            var result = await _productService.CreateProductAsync(productDto);
            if (!result.IsSuccess)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError, 
                    new Error<string>(
                        (int)HttpStatusCode.InternalServerError,
                        "Create product failed",
                        result.Message)
                );
            }

            return StatusCode((int)HttpStatusCode.Created, 
                new Success(
                    (int)HttpStatusCode.Created,
                    result.Message)
            );        
        }

        [HttpGet]
        public async Task<IActionResult> Paging(int currentPage, int pageSize)
        {
            var pagingProducts = await _productService.GetPagingProductListAsync(currentPage, pageSize);
       
            if(pagingProducts != null)
            {
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    IsSuccess = true,
                    Message = "Get list success",
                    Data = pagingProducts,
                });
            }

            return NotFound(new
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                IsSuccess = false,
                Message = "Get list fail !",
            });
        }

        [HttpGet]
        [Route("total-products")]
        public async Task<IActionResult> TotalItems()
        {
            var totalItem = await _productService.GetTotalItems();
            return Ok(new
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Get total success !",
                Data = totalItem,
            });
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            if (productDto == null)
            {
                return NotFound(new Error<string>(
                        (int)HttpStatusCode.NotFound,
                        "Create Failed",
                        "Product can not be null")
                    );
            }          
            var searchResult = await _productService.GetProductDtoByIdAsync(productDto.Id);
            var productEntity = searchResult.Item1;
            if (productEntity == null)
            {
                return NotFound(new
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    IsSuccess = true,
                    Message = "Product that needs updating cannot found !",
                });
            }
            productDto.ModifiedDate = DateTime.Now;
            var result = await _productService.UpdateProductAsync(productDto);
            var isSuccess = result.Item1;
            var message = result.Item2;
            var data = new List<ProductDto> {productEntity, productDto };
            if (isSuccess)
            {
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    IsSuccess = true,
                    Message = $"{message}", 
                    Data = data
                });
            }
            return BadRequest(new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = $"{message}"
            });
        }

        [Authorize]
        [HttpDelete]
        [Route("{productId}")]
        public async Task<IActionResult> Delete(string productId)
        {
            if (!productId.IsConvertToGuid())
            {
                return BadRequest(new
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Product Id Is InValid ! Delete Failed !"
                });
            }

            var tuple = _productService.GetProductDtoByIdAsync(new Guid(productId));
            var productObject = tuple.Result.Item1;
            if (productObject == null)
            {
                return NotFound(new
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    IsSuccess = false,
                    Message = $"Product not found ! Delete Fail !",
                });
            }
            var result = await _productService.DeleteProductAsync(new Guid(productId));
            var isSuccess = result.Item1;
            var message = result.Item2;
            if (isSuccess)
            {
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.NoContent,
                    IsSuccess = true,
                    Message = $"{message}",
                    Data = productObject
                });
            }
            return BadRequest(new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                IsSuccess = false,
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
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Product Id Is InValid !"
                });
            }
            var result = await _productService.GetProductDtoByIdAsync(new Guid(productId));
            var productDto = result.Item1;
            var message = result.Item2;
            if (productDto != null)
            {
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.NoContent,
                    IsSuccess = true,
                    Message = $"{message}",
                    Data = productDto
                });
            }
            return BadRequest(new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = $"{message}"
            });

        }
    }
}
