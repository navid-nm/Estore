using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Estore.Data;
using Estore.Models;

namespace Estore.Controllers
{
    public class PurchasedItemController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly EstoreDbContext _context;

        public PurchasedItemController(IWebHostEnvironment hostenv, EstoreDbContext context)
        {
            _env = hostenv;
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index(string findcode)
        {
            User user = _context.Users.First(u => u.Username == User.Identity.Name);
            Item item = _context.Items.FirstOrDefault(i => i.BuyerId == user.Id && i.FindCode == findcode);
            if (item != null)
            {
                item.ImageUrls = new ItemData(_context, _env).GetImages(item);
                ViewBag.PurchasedItem = item;
                return View();
            }
            return Redirect("/dashboard");
        }
    }
}
