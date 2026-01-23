using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyCv.Models;
using MyCv.Models.View;
using System.Data;

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
        public async Task<List<ProjectDetail>> GetProjectDetailsAsync(string projectId, AppDBContext _context)
        {
            return await _context.ProjectDetails
                .FromSqlRaw("EXEC GetProjectDetails @projectId", new SqlParameter("@projectId", projectId)).ToListAsync();
        }

        public static async Task CreateProject(vwProject project, AppDBContext _context)
        {
            // 1. DataTable'ı SQL'deki tiple (ProjectDetailsType) tam uyumlu oluşturuyoruz
            DataTable detailsTable = new DataTable();
            detailsTable.Columns.Add("id", typeof(int));             // 1. Kolon
            detailsTable.Columns.Add("projectId", typeof(string));  // 2. Kolon
            detailsTable.Columns.Add("type", typeof(string));       // 3. Kolon
            detailsTable.Columns.Add("visibleContent", typeof(string)); // 4. Kolon
            detailsTable.Columns.Add("content", typeof(string));    // 5. Kolon
            detailsTable.Columns.Add("subContent", typeof(string)); // 6. Kolon
            detailsTable.Columns.Add("deleteId", typeof(bool));     // 7. Kolon

            // 2. Listeyi doldururken SP'de zaten atanacak olanları boş geçebiliriz (Placeholder)
            if (project.Details != null)
            {
                foreach (var detail in project.Details)
                {
                    detailsTable.Rows.Add(
                        DBNull.Value,          // id (SQL tarafında kullanılmıyor olabilir ama şema için gerekli)
                        null,                  // projectId (SP içinde @newProjectId atanacak)
                        detail.Type,           // type
                        detail.VisibleContent, // visibleContent
                        detail.Content,        // content
                        detail.SubContent,     // subContent
                        false                  // deleteId (Varsayılan false/0)
                    );
                }
            }

            var detailsParam = new SqlParameter("@details", detailsTable)
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.ProjectDetailsType"
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC CreateProject @title, @description, @coverImgUrl, @tags, @details",
                new SqlParameter("@title", project.title ?? (object)DBNull.Value),
                new SqlParameter("@description", project.description ?? (object)DBNull.Value),
                new SqlParameter("@coverImgUrl", project.coverImgUrl ?? (object)DBNull.Value),
                new SqlParameter("@tags", project.tags ?? (object)DBNull.Value),
                detailsParam
            );
        }
    }
}
