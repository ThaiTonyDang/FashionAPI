using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

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

        public async Task<bool> CreateUserAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }

        public async Task<IEnumerable<string>> GetListRoles(User user)
        {
            var data = await _userManager.FindByIdAsync(user.Id.ToString());
            var roles = await _userManager.GetRolesAsync(data);
            return roles.AsEnumerable();
        }

        public Task<User> GetUserByEmail(string email)
        {
            return this._userManager.FindByEmailAsync(email);
        }

        public async Task<bool> ValidationUser(User user, string password)
        {
            var existedUser = await GetUserByEmail(user.Email);
            var result = existedUser != null && await this._userManager.CheckPasswordAsync(existedUser, password);
            return result;
        }
    }
}
