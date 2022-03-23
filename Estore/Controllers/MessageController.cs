using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Estore.Data;
using Estore.Models;

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

        /// <summary>
        /// Set temporary data for message view.
        /// </summary>
        /// <param name="kvps">Collection containing the data to keep across multiple requests</param>
        private void SetMessageObjects(Dictionary<string, object> kvps)
        {
            foreach (string key in kvps.Keys)
            {
                TempData[key] = JsonConvert.SerializeObject(kvps[key]);
                TempData.Keep(key);
            }
        }

        private T GetMessageObject<T>(string key)
        {
            return JsonConvert.DeserializeObject<T>(TempData.Peek(key).ToString());
        }

        [HttpGet, Authorize]
        public IActionResult Index(string username, string findcode)
        {
            SetMessageObjects(new Dictionary<string, object>
            {
                { "Viewer", _context.Users.First(u => u.Username == User.Identity.Name) },
                { "Seller", _context.Users.FirstOrDefault(u => u.Username == username) },
                { "Item", new ItemData(_context, _env).GetItem(findcode) }
            });
            return View();
        }

        [HttpPost]
        public IActionResult Index(Message message)
        {
            if (ModelState.IsValid)
            {
                new MessageData(_context).AddMessage(
                    message.MessageBody, 
                    GetMessageObject<User>("Viewer"),
                    GetMessageObject<User>("Seller"),
                    GetMessageObject<Item>("Item")
                );
                return View("Success");
            }
            return View();
        }

        [HttpGet, Authorize]
        public IActionResult ViewAll()
        {
            ViewBag.Messages = new MessageData(_context).GetMessages(User.Identity.Name);
            return View("All");
        }

        [HttpGet, Authorize]
        public IActionResult ShowMessage(int id)
        {
            Message msg = new MessageData(_context, _env).GetMessage(id);
            if (msg.RecipientId == _context.Users.First(u => u.Username == User.Identity.Name).Id)
            {
                ViewBag.ThisMessage = msg;
                ViewBag.Seller = _context.Users.First(u => u.Id == msg.SubjectItem.UserId);
            }
            return View("Message");
        }
    }
}
