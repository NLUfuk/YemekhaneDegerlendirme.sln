namespace Yemekhane.Data.Repositories;

using Yemekhane.Entities;
using System.Linq;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context) => _context = context;

    public User? GetByCredentials(string username, string passwordHash)
        => _context.Users.FirstOrDefault(u => u.UserName == username && u.PasswordHash == passwordHash);

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
}
