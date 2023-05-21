﻿using Domain.Dtos;
using Domain.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) 
        {
            this._userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (user == null)
            {
                return BadRequest("User can not be null");
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                return BadRequest("Email can not be null or empty");
            }

            if (!user.Password.Equals(user.ConfirmPassword))
            {
                return BadRequest("Password confirm is not match");
            }

            var result = await this._userService.RegisterUserAsync(user);
            if (!result)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = "Can't create new user"
                });
            }

            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest("User can not be null");
            }
            
            if(string.IsNullOrEmpty(user.Email))
            {
                return BadRequest("Email can not be null or empty");
            }

            if (!user.Password.Equals(user.Password))
            {
                return BadRequest("Password can not be null or empty");
            }

            return Ok();
        }
    }
}
