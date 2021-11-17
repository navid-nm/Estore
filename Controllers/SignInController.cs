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
            return View();
        }

        [HttpPost]
        public IActionResult Index(SignInViewModel svm)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("Name", svm.Username);
                return Redirect("/");
            }
            return View();
        }
    }
}
