using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCv.Models;
using MyCv.Models.View;
using MyCv.ViewModels;


namespace MyCv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        AppDBContext context;
        public AdminController(AppDBContext _context) 
        {
            context =   _context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Index");
        }
        
        [HttpGet("Login")]
        public async Task<IActionResult> Login(string nickName, string Password)
        {
            var result = AdminDBOp.Login(new User
            {
                NickName = nickName,
                Password = Password
            }, context).Result;
            return Ok(result);
        }

    }
}
