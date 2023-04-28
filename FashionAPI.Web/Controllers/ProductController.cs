using FashionWebAPI.Infrastructure.Models;
using FashionWebAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<IActionResult> Creat(Product product)
        {
            var isSuccess = await _productRepository.AddAsync(product);

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
    }
}
