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

        private void SetListingViewData(long idl)
        {
            Item newitem = item.Get(idl);
            if (item != null)
            {
                //ViewBag is used for the item to avoid having to repeatedly cast to Item in the view
                ViewBag.ThisItem = newitem;
                ViewData["IgnoreControllerValidation"] = false;
                item.IncrementViews(idl);
            }
        }

        public IActionResult Index(string id, Bid bid)
        {
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
                var preget = item.Get(idl);
                curprice = preget.Price;

                string seller = preget.Username;
                string sessionname = HttpContext.Session.GetString("Name");

                ViewData["BidPlaceAttempt"] = true;
                if (sessionname == null) ViewData["Error"] = "Sign in to place a bid.";
                else if (bid.Amount <= curprice) ViewData["Error"] = "Your bid must be higher than the current bid.";
                else if (sessionname == seller) ViewData["Error"] = "You cannot bid on your own item.";
                else
                {
                    if (ModelState.IsValid)
                    {
                        item.AddBid(new Bid { ID = idl, Username = sessionname, Amount = bid.Amount });
                    }
                    else
                    {
                        SetListingViewData(idl);
                        ViewData["IgnoreControllerValidation"] = true;
                        return View();
                    }
                }
            }
            if (pass) SetListingViewData(idl);
            return View();
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
