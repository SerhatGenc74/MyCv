using MyCv.Models;

namespace MyCv.ViewModels
{
    public class AdminDBOp
    {
        public static List<Content> GetList(AppDBContext context)
        {
            return context.Contents.ToList();
        }
    }
}
