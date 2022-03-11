using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Estore.Data;
using Estore.Models;

namespace Estore.Controllers
{
    public class ItemController : Controller
    {
        private readonly EstoreDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ItemController(EstoreDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public IActionResult Index(string findcode)
        {
            using var idata = new ItemData(_context, _env);
            using var udata = new UserData(_context);
            Item item = idata.GetItem(findcode);
            if (User.Identity.Name != null)
            {
                udata.WriteViewed(User.Identity.Name, item);
                ViewBag.ViewingUser = _context.Users.First(u => u.Username == User.Identity.Name);
            }
            if (item != null) ViewBag.ThisItem = item;
            return View(_context.Users.ToList());
        }
    }
}
