using System.ComponentModel.DataAnnotations;

namespace Users.Models
{
    public class RegisterUserModel
    {
        [Required]
        [RegularExpression(@"^\p{L}+[ \s]?\p{L}+$", ErrorMessage = "Ogiltigt förnamn")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^\p{L}+[ \s]?\p{L}+$", ErrorMessage = "Ogiltigt förnamn")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[a-z\d._%+-]+@[a-z\d.-]+\.[a-z]{2,}$", ErrorMessage = "E-postadressen är ogiltig")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.{6,}$)(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*\W).*$", ErrorMessage = "Ogiltigt lösenord")]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"^\+\d{2}-?\d{2}-?\d{3}\s?\d{2}\s?\d{2}|\d{2,3}-?\d{3}\s?\d{2}\s?\d{2}$", ErrorMessage = "Telefonnumret är ogiltigt")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^\p{L}([-\p{L} ]+)? \d+$", ErrorMessage = "Ogiltig adress")]
        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^\p{L}+([ \s]\p{L}+)?$", ErrorMessage = "Ogiltig stad")]
        public string City { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}\s?\d{2}$", ErrorMessage = "Postnummret är ogiltigt")]
        public string Zip { get; set; }
    }
}
