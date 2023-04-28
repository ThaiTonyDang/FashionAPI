using FashionWebAPI.Infrastructure.Models;
using FashionWebAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FashionAPI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetListProducts()
        {
            try
            {
                var products = await _productRepository.GetListProducts();

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

        [HttpPut]
        [Route("{productId}")]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            var productEntity = await _productRepository.GetProductByIdAsync(product.Id);
            if (productEntity == null)
            {
                return NotFound();
            }

            var isSuccess = await _productRepository.UpdateAsync(productEntity);

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
