using Microsoft.AspNetCore.Mvc;

namespace Estore.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
