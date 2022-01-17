using System.ComponentModel.DataAnnotations;

namespace Estore.Models
{
    public class SignIn
    {
        [Required, Display(Name = "password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, Display(Name = "email")]
        public string Email { get; set; }
    }
}
