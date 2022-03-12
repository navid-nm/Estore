using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Estore.Data;
using Estore.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Estore.Controllers
{
    public class MessageController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly EstoreDbContext _context;

        public MessageController(IWebHostEnvironment hostenv, EstoreDbContext context)
        {
            _env = hostenv;
            _context = context;
        }

        private void SaveAllMessageViewData()
        {
            foreach (string key in new List<string> { "Viewer", "Seller", "Item" })
            {
                TempData.Keep(key);
            }
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult Index(string username, string findcode)
        {
            User seller = _context.Users.FirstOrDefault(u => u.Username == username);
            Item item = new ItemData(_context, _env).GetItem(findcode);
            TempData["Viewer"] = JsonConvert.SerializeObject(_context.Users.First(u => u.Username == User.Identity.Name));
            TempData["Seller"] = JsonConvert.SerializeObject(seller);
            TempData["Item"] = JsonConvert.SerializeObject(item);
            SaveAllMessageViewData();
            return View();
        }

        [HttpPost]
        public IActionResult Index(Message message)
        {
            Debug.WriteLine("###################################################");
            //Debug.WriteLine(((User)TempData["Viewer"]).Username);
            Debug.WriteLine("###################################################");
            if (ModelState.IsValid)
            {
                new MessageData(_context).AddMessage(message.MessageBody, 
                                                     (User)TempData["Viewer"],
                                                     (User)TempData["Seller"],
                                                     (Item)TempData["Item"]);
                return Redirect("/");
            }
            return View();
        }
    }
}
