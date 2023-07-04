using Domain.DTO;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities.GlobalHelpers;

namespace API.Controllers
{
    [Authorize]
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
        [Route("ordercreate")]
        public async Task<IActionResult> Create(OrderDto orderDto)
        {
            var message = "";
            if (orderDto == null)
            {
                return BadRequest(new
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    IsSuccess = false,
                    Message = "The order to create is not available!"
                });
            }

            var result = await _orderService.CreateOrderAsync(orderDto);
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

        [HttpGet]
        [Route("orders")]
        public async Task<IActionResult> GetListOrder()
        {
            var orders = await _orderService.GetAggregatedOrderListAsync();
            if (orders.Count == 0)
            {
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    IsSuccess = false,
                    Message = "Not Orders Yet !",
                    Data = orders,
                });
            }
            return Ok(new
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Get list success",
                Data = orders,
            });
        }

        [HttpGet]
        [Route("orderdetails")]
        public async Task<IActionResult> GetOrderDetailList()
        {
            var orderdetails = await _orderService.GetAggregatedOrderDetailListAsync();
            if (orderdetails.Count == 0)
            {
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    IsSuccess = false,
                    Message = "Not Orders Yet !",
                });
            }
            return Ok(new
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Get list success",
                Data = orderdetails,
            });
        }

        [HttpGet]
        [Route("products/{orderId}")]
        public async Task<IActionResult> GetOrderedProduct(string orderId)
        {
            if (!orderId.IsConvertToGuid())
            {
                return BadRequest(new
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Order Id Is InValid !"
                });
            }
            var result = await _orderService.GetOrderedProductListAsync(new Guid(orderId));
            var products = result.Item1;
            var message = result.Item2;
            if (products == null)
            {
                return NotFound(new
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    IsSuccess = false,
                    Message = message,
                });
            }
            return Ok(new
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Message = message,
                Data = products,
            });
        }

        [HttpGet]
        [Route("customers/{orderId}")]
        public async Task<IActionResult> GetOrderedBaseInformation(string orderId)
        {
            if (!orderId.IsConvertToGuid())
            {
                return BadRequest(new
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Order Id Is InValid !"
                });
            }
            var result = await _orderService.GetOrderedBaseInformationAsync(new Guid(orderId));
            var baseInformation = result.Item1;
            var message = result.Item2;
            if (baseInformation == null)
            {
                return NotFound(new
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    IsSuccess = false,
                    Message = message,
                });
            }
            return Ok(new
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Message = message,
                Data = baseInformation,
            });
        }

        [HttpPut]
        [Route("paid-status-update")]
        public async Task<IActionResult> UpdatePayStatus(OrderDto orderDto)
        {

            var result = await _orderService.UpdateOrderPaidStatusAsync(orderDto.Id);
            if (result)
            {
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    IsSuccess = true,
                });
            }
            return NotFound(new
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                IsSuccess = false,
            });
        }
    }
}
