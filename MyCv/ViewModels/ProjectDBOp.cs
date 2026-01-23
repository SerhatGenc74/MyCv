using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyCv.Models;
using MyCv.Models.View;

namespace MyCv.ViewModels
{
    public class ProjectDBOp
    {
        public static async Task<List<Project>> ToList(AppDBContext _context)
        {
            List<Project> list = null;
            list = await _context.Projects.FromSqlRaw("EXEC GetAllProjects").ToListAsync();
            return list;
        }
        public static async Task CreateProject(vwProject project,AppDBContext _context)
        {

            await _context.Database.ExecuteSqlRawAsync("EXEC CreateProject @title, @description, @coverImgUrl, @tags, @details",
                new SqlParameter ("@title", project.title),
                new SqlParameter ("@description", project.description),
                new SqlParameter ("@coverImgUrl", project.coverImgUrl),
                new SqlParameter ("@tags",project.tags));

        }
    }
}
