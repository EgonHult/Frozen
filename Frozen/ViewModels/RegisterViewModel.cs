using System.ComponentModel.DataAnnotations;

namespace Frozen.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ange epostadress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ange lösenord")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bekräfta lösenord")]
        public string RePassword { get; set; }

        [Required(ErrorMessage = "Ange förnamn")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Ange efternamn")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Ange adress")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Ange stad")]
        public string City { get; set; }

        [Required(ErrorMessage = "Ange postnummer")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "Ange telefonummer")]
        public string Phone { get; set; }
    }
}
