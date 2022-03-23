using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Estore.Data;
using Estore.Models;

namespace Estore.Controllers
{
    public class StorefrontController : Controller
    {
        private readonly EstoreDbContext _context;

        public StorefrontController(EstoreDbContext context)
        {
            _context = context;
        }

        [HttpGet, Authorize]
        public IActionResult Add(int id)
        {
            User user = _context.Users.First(u => u.Username == User.Identity.Name);
            Storefront sf = new StorefrontData(_context).GetStorefront(id);
            if (user.Id == sf.UserId)
            {
                HttpContext.Session.SetInt32("Storefront", sf.Id);
                ViewBag.StorefrontToEdit = sf;
                return View("AppendItem");
            }
            return Redirect("/");
        }

        [HttpPost]
        public IActionResult Add(Item item)
        {
            if (ModelState.IsValid)
            {
                int id = (int)HttpContext.Session.GetInt32("Storefront");
                new StorefrontData(_context).AddItem(item, id);
                return Redirect("/storefront/" + id.ToString());
            }
            return View("AppendItem", item);
        }

        [HttpGet]
        public IActionResult Show(int id)
        {
            ViewBag.Storefront = new StorefrontData(_context).GetStorefront(id);
            ViewBag.ThisUser = _context.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
            return View("SingleStorefront");
        }

        [HttpGet, Authorize]
        public IActionResult Index()
        {
            User user = _context.Users.First(u => u.Username == User.Identity.Name);
            ViewBag.Storefronts = _context.Storefronts.Where(s => s.UserId == user.Id).ToList();
            return View();
        }

        [HttpGet, Authorize]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(Storefront sf)
        {
            if (ModelState.IsValid)
            {
                new StorefrontData(_context).AddStorefront(sf, User.Identity.Name);
                return View("Success", sf);
            }
            return View("Create", sf);
        }
    }
}
