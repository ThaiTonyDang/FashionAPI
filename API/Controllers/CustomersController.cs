using Domain.DTO;
using Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerDto customerDto)
        {
            var message = "";
            if (customerDto == null)
            {
                return NotFound(new
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                    Message = "Customer Cannot Be Empty !"
                });
            }

            var result = await _customerService.CreateCustomerAsync(customerDto);
            var isSuccess = result.Item1;
            message = result.Item2;
            if (isSuccess)
                return Ok(new
                {
                    StatusCode = HttpStatusCode.Created,
                    Success = true,
                    Message = $"{message}"
                });

            return BadRequest(new
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Message = $"{message}",
            });
        }
    }
}
