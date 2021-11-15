using System;

namespace AuctionSystemPOC.Models
{
    public class Bid
    {
        public long ID { get; set; }
        public int Amount { get; set; }
        public DateTime DateMade { get; set; }
        public string Username { get; set; }
    }
}
