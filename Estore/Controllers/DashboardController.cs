using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Estore.Models;
using Estore.Data;

namespace Estore.Controllers
{
    public class DashboardController : Controller
    {
        private readonly EstoreDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DashboardController(EstoreDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult Index()
        {
            User user = _context.Users.Where(u => u.Username == User.Identity.Name).First();
            List<Item> items = _context.Items.Where(i => user.Id == i.UserId).ToList();
            using var idata = new ItemData(_context, _env);
            foreach (var item in items)
            {
                item.ImageUrls = idata.GetImages(item);
            }
            ViewBag.ItemsSoldByUser = items;
            return View();
        }
    }
}
