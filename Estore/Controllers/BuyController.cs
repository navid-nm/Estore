using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Estore.Data;
using Estore.Models;

namespace Estore.Controllers
{
    public class BuyController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly EstoreDbContext _context;

        public BuyController(IWebHostEnvironment hostenv, EstoreDbContext context)
        {
            _env = hostenv;
            _context = context;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult Index(string findcode)
        {
            if (!new UserData(_context).UserHasLocation(User.Identity.Name))
            {
                return Redirect("/location/" + findcode);
            }
            Item item = new ItemData(_context, _env).GetItem(findcode);
            if (item == null)
            {
                return Redirect("/item/404");
            }
            ViewBag.Buyer = _context.Users.First(u => u.Username == User.Identity.Name);
            ViewBag.Seller = _context.Users.First(u => u.Id == item.UserId);
            ViewBag.ItemToBuy = item;
            return View();
        }
    }
}
