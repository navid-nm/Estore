using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EcomProofOfConcept.Models;

namespace EcomProofOfConcept.Controllers
{
    public class SignUpController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Name") != null) return Redirect("/");
            return View();
        }

        [HttpPost]
        public IActionResult Index(User user)
        {
            if (ModelState.IsValid)
            {
                user.Commit();
                return View("Success");
            }
            else
            {
                return View(user);
            }
        }
    }
}
