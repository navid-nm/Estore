using System.ComponentModel.DataAnnotations;

namespace Estore.Models
{
    /// <summary>
    /// Represents a payment to be made for an item.
    /// </summary>
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
