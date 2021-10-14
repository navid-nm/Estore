using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EcomProofOfConcept.DataAccessLayers;

namespace EcomProofOfConcept.Models
{
    public class User
    {
        [Required(ErrorMessage = "Username cannot be empty")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Username should be between 3 and 40 characters")]
        public string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        private readonly UserDB udb;

        public User()
        {
            udb = new UserDB();
        }

        public void Commit()
        {
            udb.AddUser(this);
        }

        public Tuple<int, string, int, int, string> GetInfo(string name)
        {
            return udb.GetUserInfoFromName(name);
        }

        public Dictionary<string, long> GetListings(string name)
        {
            return udb.GetListings(name);
        }
    }
}
