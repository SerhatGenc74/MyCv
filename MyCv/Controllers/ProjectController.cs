using Microsoft.AspNetCore.Mvc;
using MyCv.Models;
using MyCv.Models.View;
using MyCv.ViewModels.Operations;
using MyCv.ViewModels.Services;

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
            var projects = await service.Filter("");
            return Ok(projects);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string seach)
        {
            var projects = await service.Filter(seach);
            return Ok(projects);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] vwProject project)
        {
            await service.Add(project);
            return Ok("Saved");
        }
        [HttpPost("View")]
        public async Task<IActionResult> View(string projectId)
        {
            var project = await service.GetProjectInfo(projectId);

            return Ok(project);
        }
        [HttpPost("ChangeVisibility")]
        public async Task<IActionResult> ChangeVisibility(string projectId, bool isVisible)
        {
            await service.ChangeVisibility(projectId, isVisible);
            return Ok("Visibility Changed");
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string projectId)
        {
            await service.Delete(projectId);
            return Ok("Deleted");
        }

    }
}
