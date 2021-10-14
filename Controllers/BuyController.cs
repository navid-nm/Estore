using Microsoft.AspNetCore.Mvc;
using EcomProofOfConcept.Models;

namespace EcomProofOfConcept.Controllers
{
    public class BuyController : Controller
    {
        private readonly Item item;

        public BuyController()
        {
            item = new Item();
        }

        [HttpGet]
        public IActionResult BuyItem(long id)
        {
            item.Conclude(id);
            return Redirect("/item/" + id.ToString());
        }
    }
}
