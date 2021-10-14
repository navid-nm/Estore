using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EcomProofOfConcept.Models;

namespace EcomProofOfConcept.Controllers
{
    public class SellController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Name") == null) return Redirect("/");
            return View();
        }

        [HttpPost]
        public IActionResult Index(Item item)
        {
            if (ModelState.IsValid)
            {
                item.Username = HttpContext.Session.GetString("Name");
                item.Commit();
                return View("Success");
            }
            else
            {
                return View(item);
            }
        }
    }
}
