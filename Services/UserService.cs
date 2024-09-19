using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        if (_context.Users.Any(u => u.Username == user.Username || u.Email == user.Email))
        {
            throw new InvalidOperationException("Username or email already exists");
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateUserAsync(int id, User user)
    {
        if (id != user.UserId)
        {
            throw new ArgumentException("User ID mismatch");
        }

        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.IsAdmin = user.IsAdmin;
        existingUser.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(user.Password))
        {
            existingUser.Password = user.Password; // Update plain text password
        }

        _context.Entry(existingUser).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> AuthenticateUserAsync(string username, string password)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.Username == username && u.Password == password);
    }
}
