using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Yemekhane.Business.Services.Interfaces;

namespace Yemekhane.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IConfiguration _cfg;

        public AuthController(IAuthService auth, IConfiguration cfg)
        {
            _auth = auth;
            _cfg = cfg;
        }

        // herkes erişebilsin
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Kullanıcı adı ve şifre zorunlu.");

            var user = _auth.Register(dto.UserName, dto.Password);
            return Ok(new { user.Id, user.UserName });
        }

        // HERKES ERİŞEBİLSİN, ADMIN İÇİN ÖZELLEŞTİRMEK GEREKİYORSA
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto dto)
        {
            var user = _auth.Authenticate(dto.UserName, dto.Password);
            if (user is null) return Unauthorized("Kullanıcı adı/şifre hatalı.");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var keyStr = _cfg["Jwt:Key"];
            if (string.IsNullOrEmpty(keyStr))
                return StatusCode(500, "JWT anahtarı (Jwt:Key) yapılandırılmamış.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { token = tokenStr });
        }
    }

    // basit DTO'lar
    public record UserRegisterDto(string UserName, string Password);
    public record UserLoginDto(string UserName, string Password);
}
