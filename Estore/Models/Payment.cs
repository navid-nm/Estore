using System.ComponentModel.DataAnnotations;

namespace Estore.Models
{
    public class Payment
    {
        public User Seller { get; set; }
        public User Buyer { get; set; }

        [Required]
        public PaymentMethod Method { get; set; }
    }

    public enum PaymentMethod
    {
        [Display(Name = "Paypal")]
        PAYPAL,
        [Display(Name = "Cashapp")]
        CASHAPP
    }
}
