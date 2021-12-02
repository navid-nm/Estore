using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AuctionSystemPOC.DataAccessLayers;

namespace AuctionSystemPOC.Models
{
    /// <summary>
    /// Represents an item listed for auction by a user.
    /// </summary>
    public class Item
    {
        [Required(ErrorMessage = "Listing should have a name")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Name should be descriptive, between 7 and 200 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Listing should have a description")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Description should be between 10 and 200 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Listing should have a starting price")]
        [Range(0.01, 99999998.99, ErrorMessage = "Invalid price")]
        public decimal Price { get; set; }

        [Required]
        public string Condition { get; set; }

        public string Username { get; set; }

        public bool Concluded { get; set; }

        public long ID { get; set; }

        public decimal StartingPrice { get; set; }

        public List<Bid> Bids { get; set; }

        private readonly ItemDB idb;

        public Item()
        {
            idb = new ItemDB();
        }

        public Item Get(long id)
        {
            return idb.GetItemFromID(id);
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

        public override bool Equals(object obj)
        {
            return obj.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            string cstr = Username + Name;
            return Convert.ToInt32(Price) + Int32.Parse(new string(cstr.Where(Char.IsDigit).ToArray()));
        }
    }
}
