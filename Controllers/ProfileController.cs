using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EcomProofOfConcept.Models;

namespace EcomProofOfConcept.Controllers
{
    public class ProfileController : Controller
    {
        private readonly User user;

        public ProfileController()
        {
            user = new User();
        }

        [HttpGet]
        public IActionResult GetProfile(string name)
        {
            Tuple<int, string, int, int, string> info = user.GetInfo(name);
            if (info != null)
            {
                ViewData["Username"] = name;
                ViewData["Email"] = info.Item2;
                ViewData["BuyerRating"] = info.Item3;
                ViewData["SellerRating"] = info.Item4;
                ViewData["MemberSince"] = info.Item5;
                ViewData["Listings"] = user.GetListings(name);
            }
            return View("Index");
        }

        public IActionResult RedirToCurrent()
        {
            return Redirect("/user/" + HttpContext.Session.GetString("Name"));
        }
    }
}
