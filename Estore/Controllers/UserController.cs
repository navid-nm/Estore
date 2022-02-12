using Microsoft.AspNetCore.Mvc;
using Estore.Data;
using System.Linq;

namespace Estore.Controllers
{
    public class UserController : Controller
    {
        private readonly EstoreDbContext _context;

        public UserController(EstoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string username)
        {
            ViewBag.User = _context.Users.FirstOrDefault(u => u.Username == username);
            return View();
        }
    }
}
