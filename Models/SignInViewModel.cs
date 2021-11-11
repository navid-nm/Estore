using System.ComponentModel.DataAnnotations;

namespace AuctionSystemPOC.Models
{
    public class SignInViewModel
    {
        [Required]
        public string Username { get; set; }
    }
}
