using System.ComponentModel.DataAnnotations;
using EcomProofOfConcept.DataAccessLayers;

namespace EcomProofOfConcept.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        private readonly UserDB udb;

        public LoginViewModel()
        {
            udb = new UserDB();
        }

        public string GetUsername(string email)
        {
            return udb.GetUsernameByEmail(email);
        }

        public bool Authenticate()
        {
            return udb.VerifyEmailAndPass(Email, Password);
        }
    }
}
