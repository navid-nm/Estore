using AuctionSystemPOC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuctionSystemPOC.Controllers
{
    public class SignOutController : Controller
    {
        public IActionResult Leave()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
