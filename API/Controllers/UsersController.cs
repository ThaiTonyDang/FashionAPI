using API.Dtos;
using Domain.Dtos.Users;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
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

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegistrationDto user)
        {
            if (user == null)
            {
                return BadRequest(new Error(
                        (int)HttpStatusCode.BadRequest,
                        "Register Failed",
                        new[] { "User can not be null" })
                    );
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                return BadRequest(new Error(
                        (int)HttpStatusCode.BadRequest,
                        "Register failed",
                        new[] { "Email can not be null or empty" })
                    );
            }

            if (!user.Password.Equals(user.ConfirmPassword))
            {
                return BadRequest(new Error(
                        (int)HttpStatusCode.BadRequest,
                        "Register failed",
                        new[] { "Password confirm is not match" })
                    );
            }

            var result = await this._userService.RegisterUserAsync(user);
            if (!result.IsSuccess)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Error(
                        (int)HttpStatusCode.InternalServerError,
                        "Register failed",
                        new[] { result.Message })
                );
            }

            return Ok(new Success((int)HttpStatusCode.Created, result.Message));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto user)
        {
            if (user == null)
            {
                return BadRequest(new Error(
                        (int)HttpStatusCode.BadRequest,
                        "Login failed",
                        new[] { "User can not be null or empty" })
                    );
            }
            
            if(string.IsNullOrEmpty(user.Email))
            {
                return BadRequest(new Error(
                        (int)HttpStatusCode.BadRequest,
                        "Login failed",
                        new[] { "Email can not be null or empty" })
                    );
            }

            var isValidated = await this._userService.ValidateUserAsync(user);
            if(!isValidated)
                return Unauthorized(new Error(
                        (int)HttpStatusCode.Unauthorized,
                        "Login failed",
                        new[] { "Username or Password are not correct" })
                    );

            var token = await this._userService.CreateTokenAsync(user);

            return Ok(new SuccessData<string>((int)HttpStatusCode.OK, "Login Sucessufully", token));
        }

        [Authorize]
        [HttpGet]
        [Route("profile")]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var user = await _userService.GetUserProfie(userId);

            if (user == null)
                return NotFound(new Error(
                        (int)HttpStatusCode.NotFound,
                        "Retrive user failed",
                        new[] { "User can not be found" })
                );

            return Ok(new SuccessData<UserProfileDto>((int)HttpStatusCode.OK, "Get user's info sucessfully", user));
        }

        [Authorize]
        [HttpPatch]
        [Route("profile")]
        public async Task<IActionResult> UpdateProfileAsync(UserProfileDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest(new Error(
                        (int)HttpStatusCode.BadRequest,
                        "Update user's profile failed",
                        new[] { "User can not be null or empty" })
                    );
            }

            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var result = await this._userService.UpdateUserAsync(userId, userDto);
            if (!result.IsSuccess)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Error(
                        (int)HttpStatusCode.InternalServerError,
                        "Update user failed",
                        new[] { result.Message })
                );
            }

            return Ok(new Success((int)HttpStatusCode.OK, result.Message));
        }

        [Authorize]
        [HttpPatch]
        [Route("profile/avatar")]
        public async Task<IActionResult> UpdateAvatarAsync(string avatar)
        {
            if (string.IsNullOrEmpty(avatar))
            {
                return BadRequest(new Error(
                        (int)HttpStatusCode.BadRequest,
                        "Update user's profile failed",
                        new[] { "Avatar can not be null or empty" })
                    );
            }

            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var result = await this._userService.UpdateUserAvatarAsync(userId, avatar);
            if (!result.IsSuccess)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Error(
                        (int)HttpStatusCode.InternalServerError,
                        "Update user failed",
                        new[] { result.Message })
                );
            }

            return Ok(new Success((int)HttpStatusCode.OK, result.Message));
        }

        [Authorize]
        [HttpPatch]
        [Route("profile/change-password")]
        public async Task<IActionResult> ChangePasswordAsync(UserPasswordChangeDto userPasswordDto)
        {
            if(userPasswordDto == null || !userPasswordDto.IsPasswordValidated())
            {
                return BadRequest(new Error(
                        (int)HttpStatusCode.BadRequest,
                        "Change User's Password Failed",
                        new[] { "Confirm Password does not match" })
                    );
            }

            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var result = await _userService.UpdateUserPasswordAsync(userId, userPasswordDto.CurrentPassword, userPasswordDto.NewPassword);
            if(!result.IsSuccess)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Error(
                        (int)HttpStatusCode.InternalServerError,
                        "Update user failed",
                        new[] { result.Message })
                );
            }
                
            return Ok(new Success((int)HttpStatusCode.OK, result.Message));
        }
    }
}
