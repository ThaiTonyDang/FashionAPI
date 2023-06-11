using Domain.Dtos;
using Domain.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Utilities.GlobalHelpers;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public IActionResult GetCartItems()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!userId.IsConvertToGuid())
            {
                return BadRequest(new
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "User Id Is InValid ! "
                });
            }

            var carts = _cartService.GetCartItems(new Guid(userId));
            if (carts != null)
            {
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    IsSuccess = true,
                    Message = "Get list success",
                    Data = carts,
                });
            }

            return NotFound(new
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                IsSuccess = true,
                Message = "Get list fail !",
            });
        }

        [HttpPost]
        public async Task<IActionResult> SaveCart(CartItemDto cartItemDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!userId.IsConvertToGuid())
            {
                return BadRequest(new
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "User Id Is InValid ! "
                });
            }
            var result = await _cartService.SaveCartAsyn(cartItemDto, new Guid(userId));
            var isSuccess = result.Item1;
            var message = result.Item2;
            if (isSuccess)
            {
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    IsSuccess = true,
                    Message = message,
                });
            }

            return BadRequest(new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                IsSuccess = true,
                Message = message,
            });
        }
    }
}
