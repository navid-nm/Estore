using System;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using AuctionSystemPOC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace AuctionSystemPOC.Controllers
{
    public class ListingController : Controller
    {
        private readonly Item item;

        public ListingController()
        {
            item = new Item();
        }

        public IActionResult Index(string id, decimal amount)
        {
            decimal curprice = 0;
            if (id.All(char.IsDigit))
            {
                long idl = Int64.Parse(id);
                Tuple<string, string, decimal, string, string, bool> info = item.GetInfo(idl);
                if (info != null)
                {
                    item.IncrementViews(idl);
                    ViewData["ID"] = id;
                    ViewData["Name"] = info.Item1;
                    ViewData["Description"] = info.Item2;
                    ViewData["Price"] = info.Item3;
                    ViewData["Condition"] = info.Item4;
                    ViewData["Username"] = info.Item5;
                    ViewData["Concluded"] = info.Item6;
                    curprice = info.Item3;
                }
            }
            if (HttpContext.Request.Method == HttpMethod.Post.Method)
            {
                ViewData["BidPlaceAttempt"] = true;
                if (HttpContext.Session.GetString("Name") == null)
                    ViewData["Error"] = "Sign in to place a bid.";
                else if (amount < curprice)
                    ViewData["Error"] = "This bid is lower than the current bid.";
            }
            return View("Index");
        }

        [HttpGet]
        public IActionResult All()
        {
            List<Item> items = item.GetAll();
            if (items != null) ViewData["Items"] = items;
            return View("All");
        }
    }
}
