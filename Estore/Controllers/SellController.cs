using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Estore.Data;
using Estore.Models;

namespace Estore.Controllers
{
    public class SellController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly EstoreDbContext _context;

        public SellController(IWebHostEnvironment hostenv, EstoreDbContext context)
        {
            _env = hostenv;
            _context = context;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!new UserData(_context).UserHasLocation(User.Identity.Name))
                {
                    return Redirect("/location");
                }
                HttpContext.Session.SetString("ifc", Guid.NewGuid().ToString("N")[14..]);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Index(Item item)
        {
            if (ModelState.IsValid)
            {
                item.FindCode = HttpContext.Session.GetString("ifc");
                item.Concluded = false;
                new ItemData(_context).AddItem(item, User.Identity.Name);
                HttpContext.Session.SetString("ifc", "");
                return Redirect("/item/" + item.FindCode);
            }
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage()
        {
            var path = _env.ContentRootPath + "\\wwwroot\\img\\items\\" 
                     + User.Identity.Name + "\\"
                     + HttpContext.Session.GetString("ifc");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            foreach (IFormFile file in Request.Form.Files)
            {
                if (file.Length > 2 * Math.Pow(10, 7))
                {
                    return Forbid();
                }
                var safe = string.Concat(file.FileName.Split(Path.GetInvalidFileNameChars()));
                using FileStream fs = new FileStream(Path.Combine(path, safe), FileMode.Create);
                await file.CopyToAsync(fs);
            }
            return Content("Success");
        }
    }
}
