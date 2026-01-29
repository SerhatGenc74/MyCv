using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyCv.Models;
using MyCv.Models.View;
using MyCv.ViewModels.Operations;

namespace MyCv.ViewModels.Services
{
    public class ProjectService
    {
        private readonly IMemoryCache cache;
        private const string cacheKey = "ProjectsList";
        AppDBContext context;
        public ProjectService(IMemoryCache _cache, AppDBContext _context)
        {
            cache = _cache;
            context = _context;
        }
      
        public async Task<List<Project>> Filter(string searchValue)
        {
            var allProjects = await GetCached(); 

            if (string.IsNullOrWhiteSpace(searchValue))
                return allProjects;
            var searchTerms = searchValue.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                         .Select(s => s.Trim())
                                         .ToList();
            var filtered = allProjects.Where(p =>
            {
                return searchTerms.Any(term =>
                    p.Title != null && p.Title.Contains(term, StringComparison.OrdinalIgnoreCase)
                    ||
                    p.Tags != null && p.Tags.Contains(term, StringComparison.OrdinalIgnoreCase)
                );
            }).ToList();

            return filtered;
        }
        public async Task<vwProject> GetProjectInfo(string projectId)
        {
            var project = await GetByProjectID(projectId);
            return new vwProject
            {
                ProjectId = project.ProjectId,
                title = project.Title,
                description = project.Description,
                coverImgUrl = project.CoverImgUrl,
                tags = project.Tags,
                Details = await ProjectDBOp.GetProjectDetailsAsync(projectId, context)
            };
        }
   
      
        public async Task Add(vwProject project)
        {
            await ProjectDBOp.CreateProject(project, context);
            ClearCache();
        }
        public async Task Update(vwProject project)
        {
            await ProjectDBOp.Update(project, context);
            ClearCache();
        }
        public async Task ChangeVisibility(string projectId, bool isVisible)
        {
            await ProjectDBOp.ChangeVisibility(projectId, isVisible, context);
            ClearCache();
        }
        public async Task Delete(string projectId)
        {
            await ProjectDBOp.Delete(projectId, context);
            ClearCache();
        }

        public void ClearCache()
        {
            cache.Remove(cacheKey);
        }


        private async Task<List<Project>> GetCached()
        {

            if (!cache.TryGetValue(cacheKey, out List<Project> projects))
            {
                Console.WriteLine("Proje için ön bellek oluşturuldu");
                projects = await ProjectDBOp.ToList(context);
                cache.Set(cacheKey, projects, TimeSpan.FromHours(2));
            }
            return projects;
        }
        private async Task<Project?> GetByProjectID(string projectId)
        {
            var allProjects = await GetCached(); // Önbellekten gelir
            return allProjects.FirstOrDefault(p => p.ProjectId == projectId);
        }
    }
}
