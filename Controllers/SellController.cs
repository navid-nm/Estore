using AuctionSystemPOC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace AuctionSystemPOC.Controllers
{
    public class SellController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
