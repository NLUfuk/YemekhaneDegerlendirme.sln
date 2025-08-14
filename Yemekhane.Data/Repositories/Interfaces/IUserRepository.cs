namespace Yemekhane.Data.Repositories;

using Yemekhane.Entities;

public interface IUserRepository
{
    User? GetByCredentials(string username, string passwordHash);
    void Add(User user);
}
