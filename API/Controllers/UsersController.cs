using API.Dtos;
using Domain.Dtos;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                        "Login Failed",
                        "Email can not be null or empty")
                    );
            }

            if (!user.Password.Equals(user.Password))
            {
                return BadRequest(new Error<string>(
                        (int)HttpStatusCode.BadRequest,
                        "Login Failed",
                        "Password can not be null or empty")
                    );
            }

            var isValidated = await this._userService.ValidateUserAsync(user);
            if(!isValidated)
                return Unauthorized(new Error<string>(
                        (int)HttpStatusCode.BadRequest,
                        "Login Failed",
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
    }
}
