using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Estore.Data;
using Estore.Models;

namespace Estore.Controllers
{
    public class UserController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly EstoreDbContext _context;

        public UserController(IWebHostEnvironment hostenv, EstoreDbContext context)
        {
            _env = hostenv;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string username)
        {
            User user = _context.Users.Include(u => u.Items)
                                      .FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                List<Item> items = new List<Item>();
                foreach (Item item in user.Items)
                {
                    items.Add(new ItemData(_context, _env).GetItem(item.FindCode));
                }
                ViewBag.User = user;
                ViewBag.Items = items;
            }
            return View();
        }
    }
}
