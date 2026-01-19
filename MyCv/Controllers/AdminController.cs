using Microsoft.AspNetCore.Mvc;
using MyCv.Models;
using MyCv.ViewModels;
using MyCv.ViewModels.DataBase;


namespace MyCv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var ss = AdminDBOp.GetList(context);
            return Ok(ss);
        }
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return Ok();
        }

    }
}
