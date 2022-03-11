using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace Estore.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Username should be between 3 and 40 characters")]
        [Display(Name = "username")]
        public string Username { get; set; }

        [Required, Display(Name = "first name"), StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required, Display(Name = "surname"), StringLength(50, MinimumLength = 2)]
        public string Surname { get; set; }

        [Required, Display(Name = "password"), StringLength(250, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, EmailAddress, Display(Name = "email"), StringLength(254)]
        public string Email { get; set; }

        public Location ShippingLocation { get; set; }

        public DateTime DateOfRegistration { get; set; }

        public List<Item> Items { get; set; }
    }
}
