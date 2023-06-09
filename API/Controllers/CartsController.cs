using API.Dtos;
using Domain.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using System;
using Domain.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpPost]
        public async Task<IActionResult> Save(CartItemDto cartItemDto)
        {
            var message = "";
            if (cartItemDto == null)
            {
                return NotFound(new Error<string>(
                        (int)HttpStatusCode.NotFound,
                        "Save Failed",
                        "Cart item can not be null")
                    );
            }

            var result = await _cartService.SaveCartAsyn(cartItemDto);
            var isSuccess = result.Item1;
            message = result.Item2;
            if (isSuccess)
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    IsSuccess = true,
                    Message = $"{message}",
                    Data = cartItemDto
                });

            return BadRequest(new
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = $"{message}",
            });
        }
    }
}
