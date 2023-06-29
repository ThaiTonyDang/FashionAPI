using Domain.Dtos;
using Domain.Dtos.Users;
using Infrastructure.Models;

namespace Domain.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(UserRegistrationDto user);
        Task<bool> ValidateUserAsync(UserLoginDto user);
        Task<string> CreateTokenAsync(UserLoginDto user);
        Task<UserInfoDto> GetUserInfo(string userId);
        Task<bool> UpdateUserAsync(string userId, UserInfoDto userInfo);
        Task<bool> UpdateUserAvatarAsync(string userId, string avatar);
        Task<bool> UpdateUserPasswordAsync(string userId, string password, string newPassword);
    }
}
