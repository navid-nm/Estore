using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Estore.Models;
using Estore.Data;

namespace Estore.Controllers
{
    public class HomeController : Controller
    {
        private readonly EstoreDbContext _context;
        private readonly IWebHostEnvironment _env;

        public HomeController(EstoreDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity.Name != null)
            {
                User user = _context.Users.Where(u => u.Username == User.Identity.Name).First();
                List<Item> items = new UserData(_context).GetViewed(user.Username);
                foreach (Item item in items)
                {
                    item.ImageUrls = new ItemData(_context, _env).GetImages(item);
                }
                if (items.Count > 0) ViewBag.ViewingHistory = items.Distinct().ToList();
            }            
            return View();
        }

        [HttpGet]
        public IActionResult Leave()
        {
            HttpContext.Session.Clear();
            foreach (var c in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(c);
            }
            return Redirect("/");
        }
    }
}
