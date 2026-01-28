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
        public IActionResult Login(string Username, string Password)
        {
            
            var result = AdminDBOp.Login(new User
            {
                NickName = Username,
                Password = Password
            },"", _context).Result;
            if (result == null)
            {
                return Unauthorized("Invalid username or password");
            }
            var token = GenerateJwtToken.Generate(result);
            Response.Headers.Add("Authorization", "Bearer " + token);

            return Ok(new { Token = token, Message = "Login successful" }); 
        }       
       
    }
}