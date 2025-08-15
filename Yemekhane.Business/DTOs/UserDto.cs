
namespace Yemekhane.Business.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;

        ///public string PasswordHash { get; set; } // Password hash should not be 
        // exposed in DTO
        // AMA split etmeden validatorda lazım 
    }
}
