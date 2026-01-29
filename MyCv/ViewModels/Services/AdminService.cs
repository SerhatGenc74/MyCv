using MyCv.Models;

namespace MyCv.ViewModels.Services
{
    public class AdminService
    {
        AppDBContext context;
        public AdminService(AppDBContext _context)
        {
            context = _context;
        }

    }
}
