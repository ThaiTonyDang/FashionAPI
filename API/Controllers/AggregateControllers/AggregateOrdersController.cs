using Domain.AggregateService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Utilities.GlobalHelpers;

namespace API.Controllers.AggregateControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregateOrdersController : ControllerBase
    {
        private readonly IOrderInformationService _orderManagementService;
        public AggregateOrdersController(IOrderInformationService orderManagementService)
        {
            _orderManagementService = orderManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var orders = await _orderManagementService.GetListOrderInformationAsync();

            return Ok(new
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Message = "Get list success",
                Data = orders,
            });
        }

        [HttpGet]
        [Route("{orderId}")]
        public async Task<IActionResult> GetSingleOrderDetail(string orderId)
        {
            if (!orderId.IsConvertToGuid())
            {
                return BadRequest(new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Order Id Is InValid !"
                });
            }
            var result = await _orderManagementService.GetSingleOrderDetailByIdAsync(new Guid(orderId));
            var orderDetail = result.Item1;
            var message = result.Item2;
            return Ok(new
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Message = message,
                Data = orderDetail,
            });
        }
    }
}
