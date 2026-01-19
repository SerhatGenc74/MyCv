using Microsoft.AspNetCore.Mvc;

namespace MyCv.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
