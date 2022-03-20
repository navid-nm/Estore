using System.ComponentModel.DataAnnotations;

namespace Estore.Models
{
    /// <summary>
    /// Represents the location information belonging to a user.
    /// </summary>
    public class Location
    {
        public int Id { get; set; }

        [Required, DataType(DataType.PostalCode), Display(Name = "Postal Code"), StringLength(12)]
        public string PostalCode { get; set; }

        [Required, StringLength(100)]
        public string Address { get; set; }
    }
}
