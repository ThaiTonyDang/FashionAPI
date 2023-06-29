﻿using Infrastructure.AggregateModel;
using Infrastructure.Models;

namespace Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CreateUserAsync(User user, string password);
        Task<User> GetUserByEmail(string email);
        Task<bool> ValidationUser(User user, string password);
        Task<IEnumerable<string>> GetListRoles(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> ChangeUserPasswordAsync(User user, string password, string newPassword);
        Task<User> GetUserById(string userId);
    }
}
