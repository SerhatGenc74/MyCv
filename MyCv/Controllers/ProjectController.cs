using Microsoft.AspNetCore.Mvc;
using MyCv.Models;
using MyCv.Models.View;
using MyCv.ViewModels;

namespace MyCv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        AppDBContext context;
        public ProjectController(AppDBContext _context)
        {
            context = _context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] vwProject project)
        {
            await ProjectDBOp.CreateProject(project, context);
            return Ok();
        }
    }
}
