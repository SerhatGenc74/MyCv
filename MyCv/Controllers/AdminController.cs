using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyCv.Models;
using MyCv.Models.View;
using MyCv.ViewModels.Operations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace MyCv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        IConfiguration configuration;
        AppDBContext context;
        public AdminController(IConfiguration _configuration, AppDBContext _context)
        {
            context = _context;
            configuration = _configuration;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Index");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string nickName, string Password)
        {
            var deviceId = DeviceHelper.GetOrCreateDeviceId(HttpContext);
            Console.WriteLine(deviceId);
            var result = AdminDBOp.Login(new User
            {
                NickName = nickName,
                Password = Password
            }, deviceId, context).Result;

            if (result == null)
            {
                return Unauthorized("Invalid username or password");
            }
            var token = GenerateJwtToken(result);
            Response.Headers.Add("Authorization", "Bearer " + token);

            return Ok(result);
        }
        [HttpGet("NewAdmin")]
        public async Task<IActionResult> NewAdmin(string email)
        {
            await AdminDBOp.Add(new User
            {
                Email = email
            }, context);
            return Ok(await AdminDBOp.ToList(context));
        }
        [HttpGet("ToList")]
        public async Task<IActionResult> ToList()
        {
            return Ok(await AdminDBOp.ToList(context));
        }
        [HttpPost("FirstProfileEdit")]
        public async Task<IActionResult> FirstProfileEdit([FromBody] User user)
        {
            var userID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            user.UserId = userID;
            if (userID.IsNullOrEmpty() || !await AdminDBOp.SessionControl(userID, context))
                throw new Exception("kullanıcısının oturumu kapalıdır.");

            //Giriş yapamadan ve Session Kontrol Yapmadan Edit yapamazın
            await AdminDBOp.FirstProfileEdit(user, context);
            return Ok();
        }
        [NonAction]
        public string GenerateJwtToken(string user)
        {
            var claims = new[]
            {
             new Claim(ClaimTypes.NameIdentifier, user)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
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
