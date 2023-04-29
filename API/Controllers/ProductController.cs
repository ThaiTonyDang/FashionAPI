using Domain.Services;
using Domain.ViewModels;
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

        [HttpGet]
        public async Task<IActionResult> GetListProducts()
        {
            try
            {
                var products = await _productService.GetListProductsAsync();

                if (products == null)
                {
                    return NotFound();
                }

                return Ok(products);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Creat(ProductItemViewModel product)
        {
            var isSuccess = await _productService.AddProductAsync(product);

            if (isSuccess)
            {
                return Ok(new
                {
                    Success = true,
                    Data = product
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

            var product = await _productService.GetProductItemByIdAsync(id);
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

        [HttpPut]
        [Route("{productId}")]
        public async Task<IActionResult> UpdateProduct(ProductItemViewModel product)
        {
            var productEntity = await _productService.GetProductItemByIdAsync(product.Id);
            if (productEntity == null)
            {
                return NotFound();
            }

            var isSuccess = await _productService.UpdateProductAsync(product);

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
    }
}
