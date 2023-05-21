using Domain.Dtos;
using Infrastructure.Config;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
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
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim(ClaimTypes.Email, user.UserName),
                new Claim(ClaimTypes.Email, user.UserName),
            };

            var roles = await this._userRepository.GetListRoles(user);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                _tokenConfig.Issuer,
                _tokenConfig.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_tokenConfig.Expired),
                signingCredentials: signingCredentials);
                        
            return tokenOptions;
        }
    }
}
