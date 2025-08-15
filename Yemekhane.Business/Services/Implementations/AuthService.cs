using System.Security.Cryptography;
using System.Text;
using Yemekhane.Business.Services.Interfaces;
using Yemekhane.Data.Repositories;
using Yemekhane.Entities;

namespace Yemekhane.Business.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        // Şifre hashing için SHA256 kullanacağız
        public AuthService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public User Register(string username, string password)
        {
            // Basit kontrol: aynı kullanıcı adı var mı?
            // (IUserRepository'e bir Exists metodu eklenebilirdi. Burada GetByCredentials ile password uyuşmasa da aynı user name kontrolüne uygun değil.
            // Basitlik için şimdilik doğrudan eklemeye çalışacağız.)
            string hash = HashPassword(password);
            var newUser = new User { UserName = username, PasswordHash = hash };
            _userRepo.Add(newUser);
            // Not: SaveChanges çağrısını burada yapmayacağız, API katmanında veya dışarıda yapmak mantıklı olabilir.
            return newUser;
        }

        public User? Authenticate(string username, string password)
        {
            string hash = HashPassword(password);
            return _userRepo.GetByCredentials(username, hash);
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = sha.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            // Hex string olarak SHA256 hash
        }
    }


}
