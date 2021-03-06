using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estore.Models
{
    /// <summary>
    /// Represents an item listed for sale by a user.
    /// </summary>
    public class Item
    {
        public int Id { get; set; }
        public int? BuyerId { get; set; }
        public bool Concluded { get; set; }
        public bool StorefrontItem { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        /*
         * For locating item data stored on the server but not the database (e.g. images).
         * This is used instead of ID, as the last item ID could change before the user completes 
         * the "sell" form.
         */
        [MaxLength(20)]
        public string FindCode { get; set; }

        [Required, MinLength(3), MaxLength(100)]
        public string Name { get; set; }

        [Required, Range(0.01, 70000000, ErrorMessage = "Minimum price is 0.01")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required, Range(0, 70000)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ShippingCost { get; set; }

        [Required]
        public Condition Quality { get; set; }

        [Required, MinLength(5), MaxLength(1400)]
        public string Description { get; set; }

        [NotMapped]
        public List<string> ImageUrls { get; set; }

        public string GetConditionDisplayName()
        {
            return Quality.GetType().GetMember(Quality.ToString())
                          .First()
                          .GetCustomAttribute<DisplayAttribute>()
                          .GetName();
        }
    }

    public enum Condition
    {
        [Display(Name = "New")]
        NEW,
        [Display(Name = "Used - Like New")]
        USEDLIKENEW,
        [Display(Name = "Used - Good")]
        USEDGOOD,
        [Display(Name = "Used - Acceptable")]
        USEDACCEPTABLE,
        [Display(Name = "Used - Poor")]
        USEDPOOR
    }
}
