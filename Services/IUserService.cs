using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User user);
    Task UpdateUserAsync(int id, User user);
    Task DeleteUserAsync(int id);
    Task<User> AuthenticateUserAsync(string username, string password);
}
