﻿using Infrastructure.AggregateModel;
using Infrastructure.Models;

namespace Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CreateUserAsync(User user, string password, List<string> roles);
        Task<User> GetUserByEmail(string email);
        Task<Tuple<bool, string>> ValidationUser(User user, string password);
        Task<IEnumerable<string>> GetListRoles(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> UpdateUserAvatarAsync(User user);
        Task<bool> ChangeUserPasswordAsync(PasswordModel passwordModel, string email);
    }
}
