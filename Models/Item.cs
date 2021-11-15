using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AuctionSystemPOC.DataAccessLayers;

namespace AuctionSystemPOC.Models
{
    public class Item
    {
        [Required(ErrorMessage = "Listing should have a name")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Name should be descriptive, between 7 and 200 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Listing should have a description")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Description should be between 10 and 200 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Listing should have a starting price")]
        [Range(0.01, Double.PositiveInfinity)]
        public decimal Price { get; set; }

        [Required]
        public string Condition { get; set; }

        public string Username { get; set; }

        public bool Concluded { get; set; }

        public long ID { get; set; }

        private readonly ItemDB idb;

        public Item()
        {
            idb = new ItemDB();
        }

        public Tuple<string, string, decimal, string, string, bool> GetInfo(long id)
        {
            return idb.GetItemInfoFromID(id);
        }

        public List<Item> GetAll()
        {
            return idb.GetAllItems();
        }

        public void AddBid(Bid bid)
        {
            idb.AddBid(bid);
        }

        public void IncrementViews(long id)
        {
            idb.IncrementViews(id);
        }

        public int Commit()
        {
            return idb.AddItem(this);
        }
    }
}
