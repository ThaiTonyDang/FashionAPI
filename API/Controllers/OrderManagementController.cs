using Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderManagementController : ControllerBase
    {
        private readonly IOrderInformationService _orderManagementService;
        public OrderManagementController(IOrderInformationService orderManagementService)
        {
            _orderManagementService = orderManagementService;
        }
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var products = await _orderManagementService.GetListOrderInfor();

            return Ok(new
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Message = "Get list success",
                Data = products,
            });
        }
    }
}
