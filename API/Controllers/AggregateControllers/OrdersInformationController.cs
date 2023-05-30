﻿using Domain.AggregateService;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Utilities.GlobalHelpers;

namespace API.Controllers.AggregateControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersInformationController : ControllerBase
    {
        private readonly IOrderAggregateService _orderManagementService;
        public OrdersInformationController(IOrderAggregateService orderManagementService)
        {
            _orderManagementService = orderManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var orders = await _orderManagementService.GetOrderListAsync();
            if (orders.Count == 0)
            {
                return Ok(new
                {
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Message = "Not Orders Yet !",
                    Data = orders,
                });
            }    
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
        public async Task<IActionResult> GetOrderedBaseInformation(string orderId)
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
            var result = await _orderManagementService.GetOrderedBaseInformationAsync(new Guid(orderId));
            var baseInformation = result.Item1;
            var message = result.Item2;
            if (baseInformation == null)
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
                Data = baseInformation,
            });
        }
    }
}
