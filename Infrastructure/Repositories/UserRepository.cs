using Infrastructure.AggregateModel;
using Infrastructure.Dtos;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Net.WebSockets;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public UserRepository(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResultDto> ChangeUserPasswordAsync(User user, string password, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, password, newPassword);
            if (!result.Succeeded)
            {
                return new ErrorResult(string.Join(",", result.Errors));
            }

            return new SuccessResult("Change user's password successfully");
        }

        public async Task<ResultDto> CreateUserAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return new ErrorResult(string.Join(",", result.Errors));
            }

            return new SuccessResult("Updated new user successfully");
        }

        public async Task<IEnumerable<string>> GetListRoles(User user)
        {
            var data = await GetUserById(user.Id.ToString());
            var roles = await _userManager.GetRolesAsync(data);
            return roles.AsEnumerable();
        }

        public Task<User> GetUserByEmail(string email)
        {
            return this._userManager.FindByEmailAsync(email);
        }

        public Task<User> GetUserById(string userId)
        {
            return this._userManager.FindByIdAsync(userId);
        }

        public async Task<ResultDto> UpdateUserAsync(User user)
        {
            var result = await _userManager.UpdateAsync(user);
            if(!result.Succeeded)
            {
                return new ErrorResult(string.Join(",", result.Errors));
            }

            return new SuccessResult("Updated user successfully");

        }

        public async Task<bool> ValidationUser(User user, string password)
        {
            var existedUser = await GetUserByEmail(user.Email);
            var result = existedUser != null && await this._userManager.CheckPasswordAsync(existedUser, password);
            return result;
        }
    }
}
