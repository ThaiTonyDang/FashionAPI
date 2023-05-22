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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderDto orderDto)
        {
            var message = "";
            if (orderDto == null)
            {
                return NotFound(new
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                    Message = "The order to create is not available!"
                });
            }
            var result = await _orderService.CreateOrder(orderDto);
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
