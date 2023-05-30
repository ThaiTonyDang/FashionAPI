using Domain.AggregateService;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using System;
using Utilities.GlobalHelpers;

namespace API.Controllers.AggregateControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailInformationController : ControllerBase
    {
        private readonly IOrderAggregateService _orderManagementService;
        public OrderDetailInformationController(IOrderAggregateService orderManagementService)
        {
            _orderManagementService = orderManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderDetailList()
        {
            var orderdetails = await _orderManagementService.GetOrderDetailListAsync();
            if (orderdetails.Count == 0)
            {
                return Ok(new
                {
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Message = "Not Orders Yet !",
                });
            }
            return Ok(new
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Message = "Get list success",
                Data = orderdetails,
            });
        }

        [HttpGet]
        [Route("{orderId}")]
        public async Task<IActionResult> GetOrderedProduct(string orderId)
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
            var result = await _orderManagementService.GetOrderedProductListAsync(new Guid(orderId));
            var products = result.Item1;
            var message = result.Item2;
            if (products == null)
            {
                return NotFound(new
                {
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Message = message,
                });
            }
            return Ok(new
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Message = message,
                Data = products,
            });
        }
    }
}
