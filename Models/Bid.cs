using System;

namespace AuctionSystemPOC.Models
{
    public class Bid
    {
        public long ID { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateMade { get; set; }
        public string Username { get; set; }
    }
}
