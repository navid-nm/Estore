using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estore.Models
{
    public class Storefront
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [StringLength(400)]
        public string SummaryText { get; set; }

        public List<Item> Items { get; set; }
    }
}
