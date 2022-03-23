using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estore.Controllers
{
    public class PreferenceController : Controller
    {
        [HttpGet, Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
