using System;
using System.ComponentModel.DataAnnotations;

namespace AuctionSystemPOC.Models
{
    public class Bid
    {
        public long ID { get; set; }

        [Required]
        [Range(0.01, 999999998.99, ErrorMessage = "Invalid price")]
        public decimal Amount { get; set; }
        
        public DateTime DateMade { get; set; }
        public string Username { get; set; }
    }
}
