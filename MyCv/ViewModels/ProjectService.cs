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
        public async Task<Project?> GetProjectByIdAsync(string projectId)
        {
            var allProjects = await GetCachedProjectsAsync(); // Önbellekten gelir
            return allProjects.FirstOrDefault(p => p.ProjectId == projectId);
        }
        public async Task<List<Project>> FilterProjects(string searchValue)
        {
            var allProjects = await GetCachedProjectsAsync(); // Önbellekten (RAM) geliyor

            if (string.IsNullOrWhiteSpace(searchValue))
                return allProjects;

            // 1. Arama değerini virgüllere göre parçala ve temizle (boşlukları sil)
            var searchTerms = searchValue.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                         .Select(s => s.Trim())
                                         .ToList();

            // 2. Filtreleme işlemi
            var filtered = allProjects.Where(p =>
            {
                // Her bir kelime için kontrol et
                return searchTerms.Any(term =>
                    // Koşul 1: Başlıkta geçiyor mu?
                    (p.Title != null && p.Title.Contains(term, StringComparison.OrdinalIgnoreCase))
                    ||
                    // Koşul 2: Etiketlerin içinde geçiyor mu?
                    (p.Tags != null && p.Tags.Contains(term, StringComparison.OrdinalIgnoreCase))
                );
            }).ToList();

            return filtered;
        }
        public void ClearCache()
        {
            cache.Remove(cacheKey);
        }
    }
}
