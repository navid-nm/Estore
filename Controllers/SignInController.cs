using AuctionSystemPOC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace AuctionSystemPOC.Controllers
{
    public class SignInController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Tried"] = false;
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username)
        {
            ViewData["Tried"] = true;
            if (username.Length > 2)
            {
                HttpContext.Session.SetString("Name", username);
                return Redirect("/");
            }
            return View();
        }
    }
}
