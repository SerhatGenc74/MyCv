using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyCv.Models;
using MyCv.Models.View;
using System.Data;

namespace MyCv.ViewModels
{
    public class AdminDBOp
    {
        public static async Task<string> Login(User user, AppDBContext _context)
        {


            var outParam = new SqlParameter("@userId", System.Data.SqlDbType.VarChar, 5)
            {
                Direction = System.Data.ParameterDirection.Output,
            };
            await _context.Database.ExecuteSqlRawAsync("EXEC AdminLogin @nickName, @Password, @userId OUTPUT",
             new SqlParameter("@nickName", user.NickName),
             new SqlParameter("@Password", user.Password),
            outParam);

            return outParam.Value.ToString() ?? string.Empty;
        }
        public static async Task Add(User user, AppDBContext _context)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC AddAdmin @email", new SqlParameter("@email", user.Email));
        }
        public static async Task<List<User>> ToList(AppDBContext _context)
        {
            List<User> list = null;
            list = await _context.Users.FromSqlRaw("EXEC GetAllAdmins").ToListAsync();

            return list;
        }
        public static async Task FirstProfileEdit(User user, AppDBContext _context)
        {
            var userId = new SqlParameter("@userId", user.UserId);
            var nickName = new SqlParameter("@nickName", user.NickName);
            var password = new SqlParameter("@password", user.Password);

            await _context.Database.ExecuteSqlRawAsync("EXEC NewAdminFirstInfoEdit @userId, @nickName, @password",
                userId,
                nickName,
                password);
        }
        public static async Task<bool> SessionControl(string userId, AppDBContext _context)
        {

            var outParam = new SqlParameter("@isSessionOpen", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            await _context.Database.ExecuteSqlRawAsync("EXEC SessionControl @userId, @isSessionOpen OUTPUT",
                new SqlParameter("@userId", userId),
                outParam);
            return (bool)(outParam.Value ?? false);
        }
    }
}