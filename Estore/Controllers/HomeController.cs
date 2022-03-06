using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Estore.Models;
using Estore.Data;

namespace Estore.Controllers
{
    public class HomeController : Controller
    {
        private readonly EstoreDbContext _context;
        
        public HomeController(EstoreDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity.Name != null)
            {
                User user = _context.Users.Where(u => u.Username == User.Identity.Name).First();
                List<Item> items = new UserData(_context).GetViewed(user.Username);
                if (items.Count > 0) ViewBag.ViewingHistory = items;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }
    }
}
