using API.Dtos;
using Domain.Dtos;
using Domain.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
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
        public async Task<IActionResult> Login([FromBody] UserDto user)
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

            var result = await this._userService.ValidateUserAsync(user);
            var isValidated = result.Item1;
            var message = result.Item2;
            if (!isValidated)
                return Unauthorized(new Error<string>(
                        (int)HttpStatusCode.BadRequest,
                        message,
                        "Login Fail !"
                        )
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
        [HttpPut]
        [Route("avatar-update")]
        public async Task<IActionResult> UpdateUserAvatar(UserDto userDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (userDto == null)
            {
                return BadRequest(new Error<string>(
                        (int)HttpStatusCode.BadRequest,
                        "Update Failed",
                        "User can not be null or empty")
                    );
            }

            var isSuccess = await this._userService.UpdateUserAvatarAsync(userDto, email);
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
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateUser(UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest(new Error<string>(
                        (int)HttpStatusCode.BadRequest,
                        "Update Failed",
                        "User can not be null or empty")
                    );
            } 
            var isSuccess = await this._userService.UpdateUserAsync(userDto);
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
        [HttpGet]
        [Route("single-user")]
        public async Task<IActionResult> GetUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userService.GetUserByEmail(email);
            if(user == null)
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
        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ChangePassword(PasswordModelDto passwordModelDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _userService.ChangeUserPasswordAsync(passwordModelDto, email);
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
