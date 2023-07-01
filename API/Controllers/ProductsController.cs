using API.Dtos;
using Domain.Dtos;
using Domain.Services;
using Infrastructure.Dtos;
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProducts(int currentPage = 1, int pageSize = 10)
        {
            var products = await _productService.GetProductsAsync(currentPage, pageSize);
       
            if(products == null)
            {
                return StatusCode(
                (int)HttpStatusCode.InternalServerError,
                new Error(
                    (int)HttpStatusCode.InternalServerError,
                    "Get list failed",
                    new string[] { "Can not get list cause failed in system" }));
            }

            return Ok(new SuccessData<List<ProductDto>>(
                    (int)HttpStatusCode.OK,
                    "Get list product sucessfully",
                    products));
            
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> GetProduct(string productId)
        {
            if (!productId.IsConvertToGuid())
            {
                return BadRequest(new Error(
                        (int)HttpStatusCode.BadRequest,
                        "Get product failed",
                        new[] { "No productId is provided" })
                    );
            }

            var result = await _productService.GetProductByIdAsync(new Guid(productId));
            if (!result.IsSuccess)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new Error((int)HttpStatusCode.InternalServerError,
                    "Get product failed",
                    new[] { result.Message }));
            }

            var product = result.ToSuccessDataResult<ProductDto>().Data;
            return Ok(new SuccessData<ProductDto>(
                    (int)HttpStatusCode.OK,
                    result.Message,
                    product));

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> ProductCount()
        {
            var countItem = await _productService.GetCountAsync();

            return Ok(new SuccessData<int>(
                    (int)HttpStatusCode.OK,
                    "Get count product item sucessfully",
                    countItem));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest(
                    new Error(
                        (int)HttpStatusCode.BadRequest,
                        "Create product failed",
                        new[] { "Product can not be null" })
                    );
            }

            var result = await _productService.CreateProductAsync(productDto);
            if (!result.IsSuccess)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new Error(
                        (int)HttpStatusCode.InternalServerError,
                        "Create product failed",
                        new[] { result.Message })
                );
            }

            return StatusCode((int)HttpStatusCode.Created,
                new Success(
                    (int)HttpStatusCode.Created,
                    result.Message)
            );
        }

        [Authorize]
        [HttpPut]
        [Route("{productId}")]
        public async Task<IActionResult> Update(string productId, [FromBody] ProductDto productDto)
        {
            if(!productId.IsConvertToGuid())
            {
                return BadRequest(new Error(
                        (int)HttpStatusCode.BadRequest,
                        "Update product failed",
                        new[] { "No productId is provided" })
                    );
            }

            if (productDto == null)
            {
                return BadRequest(new Error(
                        (int)HttpStatusCode.BadRequest,
                        "Update product failed",
                        new[] { "Product can not be null" })
                    );
            }

            if (productDto.CategoryId == default)
            {
                return BadRequest(new Error(
                        (int)HttpStatusCode.BadRequest,
                        "Update product failed",
                        new[] { "Missing category" })
                    );
            }

            var result = await _productService.UpdateProductAsync(new Guid(productId), productDto);

            if (!result.IsSuccess)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new Error((int)HttpStatusCode.InternalServerError,
                    "Update Product failed",
                    new[] { result.Message }));
            }

            return Ok(new Success((int)HttpStatusCode.OK, result.Message));
        }

        [Authorize]
        [HttpDelete]
        [Route("{productId}")]
        public async Task<IActionResult> Delete(string productId)
        {
            if (!productId.IsConvertToGuid())
            {
                return BadRequest(new Error(
                        (int)HttpStatusCode.BadRequest,
                        "Delete product failed",
                        new[] { "No productId is provided" })
                    );
            }

            var result = await _productService.DeleteProductAsync(new Guid(productId));
            if (!result.IsSuccess)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new Error((int)HttpStatusCode.InternalServerError,
                    "Delete Product failed",
                    new[] { result.Message }));
            }

            return Ok(new Success((int)HttpStatusCode.OK, result.Message));
        }
    }
}
