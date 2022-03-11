using System;
using Microsoft.AspNetCore.Mvc;
using Estore.Data;
using Estore.Models;

namespace Estore.Controllers
{
    public class RegisterController : Controller
    {
        private readonly EstoreDbContext _context;

        public RegisterController(EstoreDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(User user)
        {
            if (ModelState.IsValid)
            {
                using var udata = new UserData(_context);
                var used = udata.InUse(user.Email, user.Username);
                if (used.Contains("Email"))
                    ModelState.AddModelError("Email", "Email is already in use.");
                else if (used.Contains("Username"))
                    ModelState.AddModelError("Username", "Username is already in use.");
                else
                {
                    udata.AddUser(user);
                    return View("Success");
                }
            }
            return View();
        }
    }
}
