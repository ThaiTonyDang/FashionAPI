﻿using Infrastructure.AggregateModel;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
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

        public async Task<bool> ChangeUserPasswordAsync(User user, string password, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, password, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> CreateUserAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
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

        public async Task<bool> UpdateUserAsync(User user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;

        }

        public async Task<bool> ValidationUser(User user, string password)
        {
            var existedUser = await GetUserByEmail(user.Email);
            var result = existedUser != null && await this._userManager.CheckPasswordAsync(existedUser, password);
            return result;
        }
    }
}
