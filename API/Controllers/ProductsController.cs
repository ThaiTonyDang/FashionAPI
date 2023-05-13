﻿using Domain.DTO;
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
            if (product == null)
            {
                return NotFound(new
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                    Message = "Product cannot be Empty !"
                });
            }    
         
            var result = await _productService.AddProductAsync(product);

            if (result)
                return Ok(new
                {
                    StatusCode = HttpStatusCode.Created,
                    Success = true,
                    Message = "Created !",
                }) ;

            return BadRequest(new
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Message = "Name or Provider already exists or Something Happened",
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
                Data = products,
                Message = "Get list success",
            });
        }

        [HttpPut]
        [Route("{productId}")]
        public async Task<IActionResult> Update(Product product)
        {
            var productEntity = await _productService.GetProductDtoByIdAsync(product.Id);
            if (productEntity == null)
            {
                return NotFound(new
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                    Message = "Product not found"
                });
            }

            var isSuccess = await _productService.UpdateProductAsync(productEntity);

            if (isSuccess)
            {
                return Ok(new
                {
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Message = "Update Success",                    
                });
            }

            return BadRequest(new
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Message = "Update Fail !"
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
                    Message = "Id InValid !"
                });
            }    

            var product = await _productService.GetProductDtoByIdAsync(new Guid(productId));
            if (product == null)
            {
                return NotFound(new
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                    Message = "Product Not Found !"
                });
            }

            var isSuccess = await _productService.DeleteProductAsync(new Guid(productId));

            if (isSuccess)
            {
                return Ok(new
                {
                    StatusCode = HttpStatusCode.NoContent,
                    Success = true,
                    Message = "Deleted !",
                });
            }

            return BadRequest(new
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Message = "Delete Fail !"
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
                    Message = "Id InValid !"
                });
            }

            var product = await _productService.GetProductDtoByIdAsync(new Guid(productId));

            if (product != null)
            {
                return Ok(new
                {
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = product
                });
            }

            return NotFound(new
            {
                StatusCode = HttpStatusCode.NotFound,
                Success = false,
                Message = "Product Not Found !"
            });
        }    
    }
}