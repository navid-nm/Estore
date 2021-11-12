using System;
using System.Linq;
using AuctionSystemPOC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuctionSystemPOC.Controllers
{
    public class ListingController : Controller
    {
        private readonly Item item;

        public ListingController()
        {
            item = new Item();
        }

        public IActionResult GetListing(string id)
        {
            if (id.All(char.IsDigit))
            {
                long idl = Int64.Parse(id);
                Tuple<string, string, float, string, string, bool> info = item.GetInfo(idl);
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
                }
            }
            return View("Index");
        }
    }
}
