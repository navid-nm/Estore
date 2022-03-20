using System.ComponentModel.DataAnnotations;

namespace Estore.Models
{
    /// <summary>
    /// Represents a sign in attempt by a user.
    /// </summary>
    public class SignIn
    {
        [Required, Display(Name = "password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, Display(Name = "email")]
        public string Email { get; set; }
    }
}
