using Microsoft.AspNetCore.Mvc;
using Estore.Data;
using System.Linq;

namespace Estore.Controllers
{
    public class SearchController : Controller
    {
        private readonly EstoreDbContext _context;

        public SearchController(EstoreDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string term)
        {
            var back = Redirect("/");
            if (term != null)
            {
                if (term.Trim().Length > 0)
                {
                    ViewBag.Results = _context.Items.Where(i => i.Name.Contains(term)).ToList();
                    ViewBag.SearchTerm = term;
                    return View();
                }
            }
            return back;
        }
    }
}
