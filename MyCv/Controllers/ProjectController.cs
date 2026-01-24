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
        public ProjectController(AppDBContext _context, ProjectService _service)
        {
            context = _context;
            service = _service;
        }
        ProjectService service;
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projects = await service.GetCachedProjectsAsync();
            return Ok(projects);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string seach)
        {
            var projects = await service.FilterProjects(seach);
            return Ok(projects);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] vwProject project)
        {
            await ProjectDBOp.CreateProject(project, context);
            return Ok("Saved");
        }
        [HttpPost("View")]
        public async Task<IActionResult> View(string projectId)
        {
            var pInfo = await service.GetProjectByIdAsync(projectId);
            var vwProject = await ProjectDBOp.GetProjectDetailsAsync(projectId, context);
            return Ok(vwProject);
        }

    }
}
