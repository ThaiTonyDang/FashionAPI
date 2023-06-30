using Domain.Dtos.Users;
using Infrastructure.Config;
using Infrastructure.Dtos;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.Extensions.Options;
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

        public async Task<ResultDto> RegisterUserAsync(UserRegistrationDto registerUser)
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

        public async Task<string> CreateTokenAsync(UserLoginDto user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user.Email);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }

        public async Task<bool> ValidateUserAsync(UserLoginDto userLogin)
        {
            var user = new User
            {
                Email = userLogin.Email
            };

            return await _userRepository.ValidationUser(user, userLogin.Password);
        }

        public async Task<UserInfoDto> GetUserInfo(string userId)
        {
            var user = await _userRepository.GetUserById(userId);
            var userDto = new UserInfoDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth
            };

            return userDto;
        }

        public async Task<ResultDto> UpdateUserAsync(string userId, UserInfoDto userInfo)
        {
            var user = await _userRepository.GetUserById(userId);

            user.Address = userInfo.Address;
            user.PhoneNumber = userInfo.PhoneNumber;
            user.LastName = userInfo.LastName;
            user.FirstName = userInfo.FirstName;
            user.DateOfBirth = userInfo.DateOfBirth;

            var result = await _userRepository.UpdateUserAsync(user);

            return result;
        }

        public async Task<ResultDto> UpdateUserAvatarAsync(string userId, string avatar)
        {
            var user =  await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return new ErrorResult("User is not found");
            }

            user.AvatarImage = avatar;
            var result = await _userRepository.UpdateUserAsync(user);
            return result;
        }

        public async Task<ResultDto> UpdateUserPasswordAsync(string userId, string password, string newPassword)
        {
            var user = await _userRepository.GetUserById(userId);
            if(user == null)
            {
                return new ErrorResult("User is not found");
            }

            var result = await _userRepository.ChangeUserPasswordAsync(user, password, newPassword);
            return result;
        }


        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(_tokenConfig.Secret);
            var securityKey = new SymmetricSecurityKey(secret);
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
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

        private async Task<List<Claim>> GetClaims(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            var roles = await this._userRepository.GetListRoles(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

    }
}
