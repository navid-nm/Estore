using System.ComponentModel.DataAnnotations;

namespace AuctionSystemPOC.Models
{
    public class SignInViewModel
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters")]
        public string Username { get; set; }
    }
}
