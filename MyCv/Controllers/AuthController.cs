using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
namespace MyCv.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {

            return Ok("Login successful"); 
        }
        public string GenerateJwtToken(AdminUser user)
        {

            var claims = new[]
            {
             new Claim(ClaimTypes.Name, user.Username),
             new Claim(ClaimTypes.Role, "Admin"),
             new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())   
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);
                
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public class AdminUser
        {
            public string Username { get; set; } = "admin";
            public string Password { get; set; } = "password";
        }
        public class LoginRequest
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }
    }
}