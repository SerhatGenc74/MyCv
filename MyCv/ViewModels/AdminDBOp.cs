using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyCv.Models;
using MyCv.Models.View;

namespace MyCv.ViewModels
{
    public class AdminDBOp
    {
        public static async Task<Result<string>> Login(User user, AppDBContext _context)
        {
            Result<string> result = null;

            if (string.IsNullOrEmpty(user.NickName) || string.IsNullOrEmpty(user.Password))
            {
                result.Error = new Error
                {
                    Code = "400",
                    Message = "Invalid input: NickName and Password are required."
                };

                return result;

            }

            var outParam = new SqlParameter("@userId", System.Data.SqlDbType.VarChar, 5)
            {
                Direction = System.Data.ParameterDirection.Output,
            };
            try
            {
                await _context.Database.ExecuteSqlRawAsync("EXEC AdminLogin @nickName, @Password, @userId OUTPUT",
             new SqlParameter("@nickName", user.NickName),
            new SqlParameter("@Password", user.Password),
             outParam);
            }
            catch (Exception ex)
            {

                throw;
            }





            result.Data = outParam.Value as string;
            return result;
        }
    }
}
