using Domain.DTO;
using Domain.Services;
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
    }
}
