using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FashionAPI.Web.Controllers
{
    [Route("api/[controller]/[action]/")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
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

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var isSuccess = await _productRepository.DeleteAsync(id);

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
