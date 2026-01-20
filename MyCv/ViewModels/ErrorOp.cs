using MyCv.Models;

namespace MyCv.ViewModels
{
    public class ErrorOp
    {
        private List<Error> errorlist = new List<Error>{
            new Error{ Code="400", Message="Bad Request" },
            new Error{ Code="401", Message="Unauthorized" },
            new Error{ Code="403", Message="Forbidden" },
            new Error{ Code="404", Message="Not Found" },
            new Error{ Code="500", Message="Internal Server Error" },
            new Error{ Code="502", Message="Bad Gateway" },
            new Error{ Code="503", Message="Service Unavailable" },
            new Error{ Code="504", Message="Gateway Timeout" }
        };


        public Error GetErrorByCode(string code)
        {
            return errorlist.FirstOrDefault(e => e.Code == code) ?? new Error { Code = "000", Message = "Unknown Error" };
        }

    }
}
