using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Estore.Data;
using Estore.Models;
using System.Linq;

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
            Item item = new ItemData(_context, _env).GetItem(findcode);
            if (item != null)
            {
                ViewBag.ThisItem = item;
            }
            return View(_context.Users.ToList());
        }
    }
}
