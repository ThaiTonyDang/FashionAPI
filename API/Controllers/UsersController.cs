using API.Dtos;
using Domain.Dtos;
using Domain.Dtos.Users;
using Domain.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities.GlobalHelpers;

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

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegistrationDto user)
        {
            if (user == null)
            {
                return BadRequest(new Error<string>(
                        (int)HttpStatusCode.BadRequest,
                        "Register Failed",
                        "User can not be null")
                    );
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                return BadRequest(new Error<string>(
                        (int)HttpStatusCode.BadRequest,
                        "Register Failed",
                        "Email can not be null or empty")
                    );
            }

            if (!user.Password.Equals(user.ConfirmPassword))
            {
                return BadRequest(new Error<string>(
                        (int)HttpStatusCode.BadRequest,
                        "Register Failed",
                        "Password confirm is not match")
                    );
            }

            var result = await this._userService.RegisterUserAsync(user);
            if (!result)
            {
                return BadRequest(new Error<string>(
                        (int)HttpStatusCode.InternalServerError,
                        "Register Failed",
                        "Can't create new user")
                    );
            }
            var response = new Success((int)HttpStatusCode.Created, "Created new user");
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto user)
        {
            if (user == null)
            {
                return BadRequest(new Error<string>(
                        (int)HttpStatusCode.BadRequest,
                        "Login Failed",
                        "User can not be null or empty")
                    );
            }
            
            if(string.IsNullOrEmpty(user.Email))
            {
                return BadRequest(new Error<string>(
                        (int)HttpStatusCode.BadRequest,
                        "Email Incorrect ! Try again",
                        "Email can not be null or empty")
                    );
            }

            var isValidated = await this._userService.ValidateUserAsync(user);
            if(!isValidated)
                return Unauthorized(new Error<string>(
                        (int)HttpStatusCode.BadRequest,
                        "Username or Password are not correct",
                        "Username or Password are not correct")
                    );

            var token = await this._userService.CreateTokenAsync(user);
            var response = new SuccessData<string>(
                    (int)HttpStatusCode.OK,
                    "Login Successufully",
                    token
                );
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [Route("profile")]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var user = await _userService.GetUserInfo(userId);
            if (user == null)
                return NotFound(new Error<string>(
                        (int)HttpStatusCode.NotFound,
                        "User null",
                        "User Can not found")
                    );
            return Ok(new
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Data = user
            });
        }

        [Authorize]
        [HttpPut]
        [Route("profile")]
        public async Task<IActionResult> UpdateProfileAsync(UserInfoDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest(new Error<string>(
                        (int)HttpStatusCode.BadRequest,
                        "Update Failed",
                        "User can not be null or empty")
                    );
            }

            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var isSuccess = await this._userService.UpdateUserAsync(userId, userDto);
            if (!isSuccess)
            {
                return BadRequest(new
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                });
            }

            return Ok(new
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
            });
        }

        [Authorize]
        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ChangePasswordAsync(UserPasswordChangeDto userPasswordDto)
        {
            if(userPasswordDto == null || !userPasswordDto.IsPasswordValidated())
            {
                return BadRequest(new Error<string>(
                        (int)HttpStatusCode.BadRequest,
                        "Change User's Password Failed",
                        "Confirm Password does not match")
                    );
            }

            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var result = await _userService.UpdateUserPasswordAsync(userId, userPasswordDto.CurrentPassword, userPasswordDto.NewPassword);
            if(!result)
                return BadRequest(new
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Old Password Not true"
                });
            return Ok(new
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Change Password Success"
            });
        }
    }
}
