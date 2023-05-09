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
                        return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetListProducts()
        {
            var products = await _productService.GetListProductsAsync();
            return Ok(products);
        }

        [HttpPut]
        [Route("update/{productId}")]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            if (product == null)
            {
                throw new Exception("Product Null");
            }
            var productEntity = await _productService.GetProductDtoByIdAsync(product.Id);
            if (productEntity == null)
            {
                return NotFound();
            }

            var isSuccess = await _productService.UpdateProductAsync(productEntity);

            if (isSuccess)
            {
                return Ok(new
                {
                    Success = true,
                    Data = productEntity
                });
            }

            return BadRequest(new
            {
                Success = false
            });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            var product = await _productService.GetProductDtoByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var isSuccess = await _productService.DeleteProductAsync(id);

            if (isSuccess)
            {
                return Ok(new
                {
                    Success = true,
                    Data = product
                });
            }

            return BadRequest();
        }
    }
}
