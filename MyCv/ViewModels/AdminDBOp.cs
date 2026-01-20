using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyCv.Models;
using MyCv.Models.View;

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
            
                return new Result<string>
                {
                    IsSuccess = true,
                    Data = outParam.Value.ToString() ?? string.Empty
                };
        }
    }
}
