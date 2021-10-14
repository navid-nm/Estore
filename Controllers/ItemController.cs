using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using EcomProofOfConcept.Models;

namespace EcomProofOfConcept.Controllers
{
    public class ItemController : Controller
    {
        private readonly Item item;

        public ItemController()
        {
            item = new Item();
        }

        [HttpGet]
        public IActionResult GetItem(string id)
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
