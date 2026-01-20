using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyCv.Models;
namespace MyCv.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        IConfiguration _configuration;
        private readonly AppDBContext _context;
        public AuthController(IConfiguration configuration,AppDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var account = _context.AdminUsers.FirstOrDefault(u => u.NickName == request.Username);
            if (account == null)
            {
                return Unauthorized("Invalid username or password");
            }
            var token = GenerateJwtToken(account);
            Response.Headers.Add("Authorization", "Bearer " + token);

            return Ok(new { Message = "Login successful" }); 
        }
        
        [NonAction]
        public string GenerateJwtToken(AdminUser user)
        {

            var claims = new[]
            {
             new Claim(ClaimTypes.Name, user.NickName),
             new Claim(ClaimTypes.Email, user.Email),
             new Claim(ClaimTypes.Role, "Admin"),
             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())   
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "localhost:5000",
                audience: "localhost:3000",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);
                
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
       
    }
}