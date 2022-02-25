using Microsoft.AspNetCore.Mvc;
using Estore.Data;
using Estore.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;

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

        public IActionResult Index(string term)
        {
            var back = Redirect("/");
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
            return back;
        }
    }
}
