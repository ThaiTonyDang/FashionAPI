using Domain.Dtos;
using Infrastructure.AggregateModel;
using Infrastructure.Config;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

namespace Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenConfig _tokenConfig;

        public UserService(IUserRepository userRepository, IOptions<TokenConfig> options)
        {
            _userRepository = userRepository;
            _tokenConfig = options.Value;
        }

        public async Task<bool> RegisterUserAsync(UserRegistrationDto registerUser)
        {
            var email = registerUser.Email.ToLower();
            var user = new User
            {
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                Email = email,
                UserName = email,
                PhoneNumber = registerUser.PhoneNumber,
                CreatedDate = DateTime.Now.ToUniversalTime()
            };

            var result = await _userRepository.CreateUserAsync(user, registerUser.Password);
            return result;
        }

        public async Task<string> CreateTokenAsync(UserDto user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user.Email);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }

        public async Task<bool> ValidateUserAsync(UserDto userLogin)
        {
            var user = new User
            {
                Email = userLogin.Email
            };

            return await _userRepository.ValidationUser(user, userLogin.Password);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(_tokenConfig.Secret);
            var securityKey = new SymmetricSecurityKey(secret);
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            var roles = await this._userRepository.GetListRoles(user);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials,
            List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                _tokenConfig.Issuer,
                _tokenConfig.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_tokenConfig.Expired),
                signingCredentials: signingCredentials);
                        
            return tokenOptions;
        }

        public async Task<bool> UpdateUserAsync(UserDto userDto)
        {
            var user = new User
            {
                Address = userDto.Address,
                PhoneNumber = userDto.PhoneNumber,
                LastName = userDto.LastName,
                FirstName = userDto.FirstName,
                Email = userDto.Email,
                DateOfBirth = userDto.Birthday
            };
            var result = await _userRepository.UpdateUserAsync(user);
            if(result) return true ;
            return false;
        }

        public async Task<bool> UpdateUserAvatarAsync(UserDto userDto, string email)
        {
            var user = new User
            {
                Email = email,
                AvatarImage = userDto.AvatarImage,
            };

            var result = await _userRepository.UpdateUserAvatarAsync(user);
            if (result) return true;
            return false;
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            var userDto = new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                AvatarImage = user.AvatarImage,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Birthday = user.DateOfBirth
            };

            return userDto;
        }

        public async Task<bool> ChangeUserPasswordAsync(PasswordModelDto passwordModelDto, string email)
        {
            var passModel = new PasswordModel
            {
                CurrentPassword = passwordModelDto.CurrentPassword,
                ConfirmPassword = passwordModelDto.ConfirmPassword,
                NewPassword = passwordModelDto.NewPassword
            };
            var result = await _userRepository.ChangeUserPasswordAsync(passModel, email);
            return result;
        }
    }
}
