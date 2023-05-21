using Domain.Dtos;
using Infrastructure.Models;

namespace Domain.Services
{
    public interface IUserService
    {
        public Task<bool> RegisterUserAsync(UserRegistrationDto user);

        public Task<bool> ValidateUserAsync(UserDto user);
    }
}
