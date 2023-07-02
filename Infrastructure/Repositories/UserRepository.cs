using Infrastructure.AggregateModel;
using Infrastructure.Dtos;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> ChangeUserPasswordAsync(PasswordModel passwordModel, string email)
        {
            var user = await GetUserByEmail(email);
            var result = await _userManager.ChangePasswordAsync(user, passwordModel.CurrentPassword, passwordModel.NewPassword);
            return result.Succeeded;
        }

        public async Task<bool> CreateUserAsync(User user, string password, List<string> roles)
        {
            var result = await _userManager.CreateAsync(user, password);
            await _userManager.AddToRolesAsync(user, roles);
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
            var users =  (from u in this._userManager.Users select u)
                         .Include(u => u.UserRoles)
                         .Include(u => u.Orders).ToList();
            var user = users.Where(u => u.Email.ToLower().Equals(email.ToLower())).FirstOrDefault();
            return Task.FromResult(user);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var userEntity = await _userManager.FindByEmailAsync(user.Email);
            userEntity.Address = user.Address;
            userEntity.PhoneNumber = user.PhoneNumber;
            userEntity.LastName = user.LastName;
            userEntity.FirstName = user.FirstName;
            userEntity.DateOfBirth = user.DateOfBirth;
            var result = await _userManager.UpdateAsync(userEntity);
            return result.Succeeded;

        }

        public async Task<bool> UpdateUserAvatarAsync(User user)
        {
            var userEntity = await GetUserByEmail(user.Email);
            userEntity.AvatarImage = user.AvatarImage;
            var result = await _userManager.UpdateAsync(userEntity);
            return result.Succeeded;
        }

        public async Task<bool> ValidationUserPassword(User user, string password)
        {
            var existedUser = await GetUserByEmail(user.Email);
            var result = existedUser != null && await this._userManager.CheckPasswordAsync(existedUser, password);
            return result;
        }

        public async Task<Tuple<bool, string>> ValidationUser(User user, string password)
        {
            var existedUser = await GetUserByEmail(user.Email);
            if (existedUser == null) return Tuple.Create(false, "Account is not exists !");

            var isPass = await this._userManager.CheckPasswordAsync(existedUser, password);
            if (!isPass) return Tuple.Create(false, "Password is wrong !");

            return Tuple.Create(true, "Validated success !");
        }
    }
}
