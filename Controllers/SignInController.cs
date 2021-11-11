using AuctionSystemPOC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace AuctionSystemPOC.Controllers
{
    public class SignInController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public SignInController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
