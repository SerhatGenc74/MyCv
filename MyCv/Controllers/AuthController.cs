using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyCv.Models;
using MyCv.Models.View;
using MyCv.ViewModels;
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
            
            var result = AdminDBOp.Login(new User
            {
                NickName = request.Username,
                Password = request.Password
            }, _context).Result;
            if (result == null)
            {
                return Unauthorized("Invalid username or password");
            }
            var token = GenerateJwtToken(result);
            Response.Headers.Add("Authorization", "Bearer " + token);

            return Ok(new { Token = token, Message = "Login successful" }); 
        }
        [NonAction]
        public string GenerateJwtToken(string user)
        {

            var claims = new[]
            {
             new Claim(ClaimTypes.NameIdentifier, user)   
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