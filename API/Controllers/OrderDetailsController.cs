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
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
        public OrderDetailsController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderDetailDto orderDetailDto)
        {
            var message = "";

            var result = await _orderDetailService.CreateOrderDetail(orderDetailDto);
            var isSuccess = result.Item1;
            message = result.Item2;
            if (isSuccess)
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    IsSuccess = true,
                    Message = $"{message}"
                });

            return BadRequest(new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = $"{message}",
            });
        }
    }
}
