using System.ComponentModel.DataAnnotations;

namespace Estore.Models
{
    public class Payment
    {
        [Required]
        public PaymentMethod Method { get; set; }

        public User Seller { get; set; }
        public User Buyer { get; set; }
    }

    public enum PaymentMethod
    {
        [Display(Name = "Paypal")]
        PAYPAL,
        [Display(Name = "Cashapp")]
        CASHAPP
    }
}
