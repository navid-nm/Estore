using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Estore.Data;
using Estore.Models;

namespace Estore.Controllers
{
    public class SearchController : Controller
    {
        private readonly EstoreDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SearchController(IWebHostEnvironment env, EstoreDbContext context)
        {
            _env = env;
            _context = context;
        }

        public IActionResult Index(string term, Dictionary<string, string> limiters)
        {
            if (term != null)
            {
                if (term.Trim().Length > 0)
                {
                    List<Item> items = _context.Items.Where(i => i.Name.Contains(term)).ToList();
                    foreach (Item item in items)
                    {
                        item.ImageUrls = new ItemData(_context, _env).GetImages(item);
                    }
                    ViewBag.Results = items;
                    ViewBag.SearchTerm = term;
                    return View();
                }
            }
            return Redirect("/");
        }
    }
}
