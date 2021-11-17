using System;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using AuctionSystemPOC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

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
            HttpContext.Session.SetString("ValidBid", "No");

            decimal curprice = 0;
            long idl = 0;
            bool pass = false;

            if (id.All(char.IsDigit))
            {
                idl = Int64.Parse(id);
                pass = true;
            }
            if (HttpContext.Request.Method == HttpMethod.Post.Method)
            {
                // 2 GetInfo calls are required
                // For obtaining price and seller username prior to and following validation
                var preget = item.GetInfo(idl);
                curprice = preget.Item3[1];

                string seller = preget.Item5;
                string sessionname = HttpContext.Session.GetString("Name");

                ViewData["BidPlaceAttempt"] = true;
                if (sessionname == null) ViewData["Error"] = "Sign in to place a bid.";
                else if (amount <= curprice) ViewData["Error"] = "Your bid must be higher than the current bid.";
                else if (sessionname == seller) ViewData["Error"] = "You cannot bid on your own item.";
                else
                {
                    item.AddBid(new Bid { ID = idl, Username = sessionname, Amount = amount });
                    HttpContext.Session.SetString("ValidBid", "Yes");
                }
            }
            if (pass)
            {
                Tuple<string, string, List<decimal>, string, string, bool, List<Bid>> info = item.GetInfo(idl);
                if (info != null)
                {
                    item.IncrementViews(idl);
                    ViewData["ID"] = id;
                    ViewData["Name"] = info.Item1;
                    ViewData["Description"] = info.Item2;
                    ViewData["StartingPrice"] = info.Item3[0];
                    ViewData["CurrentPrice"] = info.Item3[1];
                    ViewData["Condition"] = info.Item4;
                    ViewData["Username"] = info.Item5;
                    ViewData["Concluded"] = info.Item6;
                    ViewData["Bids"] = info.Item7;
                }
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
