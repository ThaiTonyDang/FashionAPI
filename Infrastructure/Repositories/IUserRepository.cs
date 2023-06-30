using Infrastructure.AggregateModel;
using Infrastructure.Dtos;
using Infrastructure.Models;

namespace Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<ResultDto> CreateUserAsync(User user, string password);
        Task<User> GetUserByEmail(string email);
        Task<bool> ValidationUser(User user, string password);
        Task<IEnumerable<string>> GetListRoles(User user);
        Task<ResultDto> UpdateUserAsync(User user);
        Task<ResultDto> ChangeUserPasswordAsync(User user, string password, string newPassword);
        Task<User> GetUserById(string userId);
    }
}
