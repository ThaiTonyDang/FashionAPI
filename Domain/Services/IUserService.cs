using Domain.Dtos;
using Infrastructure.Models;

namespace Domain.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(UserRegistrationDto user);
        Task<Tuple<bool, string>> ValidateUserAsync(UserDto user);
        Task<string> CreateTokenAsync(UserDto user);
        public Task<bool> UpdateUserAsync(UserDto userDto);
        public Task<bool> UpdateUserAvatarAsync(UserDto userDto, string email);
        public Task<UserDto> GetUserByEmail(string email);
        Task<bool> ChangeUserPasswordAsync(PasswordModelDto passwordModelDto, string password);
    }
}
