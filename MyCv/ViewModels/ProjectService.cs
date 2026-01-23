using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyCv.Models;
using MyCv.Models.View;

namespace MyCv.ViewModels
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
        public async Task<List<Project>> GetCachedProjectsAsync()
        {

            if (!cache.TryGetValue(cacheKey, out List<Project> projects))
            {
                Console.WriteLine("Proje için ön bellek oluşturuldu");
                projects = await ProjectDBOp.ToList(context);
                cache.Set(cacheKey, projects, TimeSpan.FromHours(2));
            }
            return projects;
        }
        public async Task<List<Project>> FilterProjects(string tag, string searchTerm)
        {
            var allProjects = await GetCachedProjectsAsync(); // Önbellekten gelir

            var filtered = allProjects.AsQueryable();

            if (!string.IsNullOrEmpty(tag))
                filtered = filtered.Where(p => p.Tags.Contains(tag));

            if (!string.IsNullOrEmpty(searchTerm))
                filtered = filtered.Where(p => p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            return filtered.ToList();
        }
        public void ClearCache()
        {
            cache.Remove(cacheKey);
        }
    }
}
