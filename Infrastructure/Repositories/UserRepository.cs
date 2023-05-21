using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CreateUserAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }

        public async Task<IEnumerable<string>> GetListRoles(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }

        public Task<User> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidationUser(User user, string password)
        {
            var existedUser = await this._userManager.FindByEmailAsync(user.Email);
            var result = existedUser != null && await this._userManager.CheckPasswordAsync(existedUser, password);
            return result;
        }
    }
}
