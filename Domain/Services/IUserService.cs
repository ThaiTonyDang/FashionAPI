using Domain.Dtos;
using Domain.Dtos.Users;
using Infrastructure.Dtos;
using Infrastructure.Models;

namespace Domain.Services
{
    public interface IUserService
    {
        Task<ResultDto> RegisterUserAsync(UserRegistrationDto user);
        Task<bool> ValidateUserAsync(UserLoginDto user);
        Task<string> CreateTokenAsync(UserLoginDto user);
        Task<UserInfoDto> GetUserInfo(string userId);
        Task<ResultDto> UpdateUserAsync(string userId, UserInfoDto userInfo);
        Task<ResultDto> UpdateUserAvatarAsync(string userId, string avatar);
        Task<ResultDto> UpdateUserPasswordAsync(string userId, string password, string newPassword);
    }
}
