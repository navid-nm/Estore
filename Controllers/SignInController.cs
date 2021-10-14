using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EcomProofOfConcept.Models;

namespace EcomProofOfConcept.Controllers
{
    public class SignInController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Name") != null) return Redirect("/");
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel lvm)
        {
            if (ModelState.IsValid && lvm.Authenticate())
            {
                HttpContext.Session.SetString("Name", lvm.GetUsername(lvm.Email));
                return Redirect("/");
            }
            else
            {
                ViewData["InvalidCredentials"] = true;
                return View();
            }
        }

        public IActionResult Leave()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

    }
}
