using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using Estore.Data;
using Estore.Models;

namespace Estore.Controllers
{
    public class SignInController : Controller
    {
        private readonly EstoreDbContext _context;

        public SignInController(EstoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public async void AllowAccess(SignIn svm)
        {
            User user = _context.Users.First(u => u.Email == svm.Email);
            var usrclaims = new List<Claim>() 
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var usridentity = new ClaimsIdentity(usrclaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(usridentity);
            await HttpContext.SignInAsync(principal);
        }

        [HttpPost]
        public IActionResult Index(SignIn svm, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (new UserData(_context).AuthenticateUser(svm))
                {
                    AllowAccess(svm);
                    if (returnUrl == null) returnUrl = "/";
                    return Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("Password", "Invalid credentials entered.");
                }
            }
            return View(svm);
        }
    }
}
