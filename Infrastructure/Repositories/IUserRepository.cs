using Infrastructure.Models;

namespace Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CreateUserAsync(User user, string password);
        Task<User> GetUserByEmail(string email);
        Task<bool> ValidationUser(User user, string password);
        Task<IEnumerable<string>> GetListRoles(User user);
    }
}
