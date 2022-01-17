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
        private readonly string ifc;

        public SellController(IWebHostEnvironment hostenv, EstoreDbContext context)
        {
            _env = hostenv;
            _context = context;
            ifc = "ItemFindCode";
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.Session.SetString(ifc, Guid.NewGuid().ToString("N")[14..]);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Index(Item item)
        {
            if (ModelState.IsValid)
            {
                item.FindCode = HttpContext.Session.GetString(ifc);
                new ItemData(_context).AddItem(item, User.Identity.Name);
                HttpContext.Session.SetString(ifc, "");
                return Redirect("/item/" + item.FindCode);
            }
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage()
        {
            var path = GetPath();
            void Setup()
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            }
            await Task.Run(Setup);
            foreach (IFormFile file in Request.Form.Files)
            {
                char[] avoid = Path.GetInvalidFileNameChars();
                var safe = string.Join("-", file.FileName.Split(avoid, StringSplitOptions.RemoveEmptyEntries))
                                 .TrimEnd('.');
                int sh = safe.LastIndexOf(".");
                if (sh > -1)
                {
                    safe = safe.Substring(0, sh) + Guid.NewGuid().ToString("N")[10..] + safe[sh..];
                }
                using FileStream fs = new FileStream(Path.Combine(path, safe), FileMode.Create);
                await file.CopyToAsync(fs);
            }
            return Content("Success");
        }

        /// <summary>
        /// Get path to images for item.
        /// Only to be used when session variable ItemFindCode is set.
        /// </summary>
        /// <returns>Path to store items in</returns>
        private string GetPath()
        {
            return _env.ContentRootPath + "\\wwwroot\\img\\items\\"
                + User.Identity.Name + "\\"
                + HttpContext.Session.GetString(ifc);
        }
    }
}
