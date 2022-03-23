using System.Linq;
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

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            User user = _context.Users.Where(u => u.Username == User.Identity.Name).First();
            using var idata = new ItemData(_context, _env);
            ViewBag.ItemsSoldByUser = idata.GetItems(i => user.Id == i.UserId);
            ViewBag.PurchasedItems = idata.GetItems(i => i.BuyerId == user.Id);
            return View();
        }
    }
}
