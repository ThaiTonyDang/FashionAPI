using Domain.Services;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
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
    }
}
