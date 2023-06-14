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
        public async Task<IActionResult> GetCartItems()
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

            var carts = await _cartService.GetCartItems(new Guid(userId));
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

            cartItemDto.UserId = new Guid(userId);
            var result = await _cartService.SaveCartAsyn(cartItemDto);
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

        [HttpDelete]
        [Route("{productId}")]
        public async Task<IActionResult> DeleteCartItem(string productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!userId.IsConvertToGuid() || !productId.IsConvertToGuid())
            {
                return BadRequest(new
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "User and Product Id Is InValid ! "
                });
            }
            var result = await _cartService.DeleteCartItemAsync( new Guid(userId), new Guid(productId));
            var isSuccess = result.Item1;
            var message = result.Item2;
            if (isSuccess)
            {
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.NoContent,
                    IsSuccess = true,
                    Message = message,
                });
            }
            return BadRequest(new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = message,
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(CartItemDto cartItemDto)
        {
            var isSuccess = await _cartService.UpdateQuantityCartItem(cartItemDto);
            if (isSuccess)
            {
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    IsSuccess = true,
                    Message = "Quantity has been updated",
                });
            }

            return BadRequest(new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = "Quantity updated fail !",
            });
        }
    }
}
