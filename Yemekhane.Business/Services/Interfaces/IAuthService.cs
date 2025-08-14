using Yemekhane.Entities;

namespace Yemekhane.Business.Services.Interfaces
{
    public interface IAuthService
    {
        User Register(string username, string password);
        User? Authenticate(string username, string password);
    }
}