using Domain.Dtos;

namespace Domain.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(UserRegistrationDto user);
        Task<bool> ValidateUserAsync(UserDto user);
        Task<string> CreateTokenAsync(UserDto user);
    }
}
