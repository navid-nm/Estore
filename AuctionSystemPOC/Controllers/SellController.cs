using AuctionSystemPOC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace AuctionSystemPOC.Controllers
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
                ViewData["ID"] = item.Commit();
                return View("Success");
            }
            return View(item);
        }
    }
}
