using System.ComponentModel.DataAnnotations;

namespace Frozen.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ange epostadress")]
        [RegularExpression(@"^[a-z\d._%+-]+@[a-z\d.-]+\.[a-z]{2,}$", ErrorMessage = "E-postadressen är ogiltig")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ange lösenord")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.{6,}$)(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*\W).*$", ErrorMessage = "Ogiltigt lösenord")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bekräfta lösenord")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.{6,}$)(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*\W).*$", ErrorMessage = "Ogiltigt lösenord")]
        [Compare("Password", ErrorMessage = "Lösenordet matchar inte!")]
        public string RePassword { get; set; }

        [Required(ErrorMessage = "Ange förnamn")]
        [RegularExpression(@"^\p{L}+[ \s]?\p{L}+$", ErrorMessage = "Ogiltigt förnamn")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Ange efternamn")]
        [RegularExpression(@"^\p{L}+[ \s]?\p{L}+$", ErrorMessage = "Ogiltigt efternamn")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Ange adress")]
        [RegularExpression(@"^\p{L}{4,} \d+$", ErrorMessage = "Ogiltig adress")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Ange stad")]
        [RegularExpression(@"^\p{L}+([ \s]\p{L}+)?$", ErrorMessage = "Ogiltig stad")]
        public string City { get; set; }

        [Required(ErrorMessage = "Ange postnummer")]
        [DataType(DataType.PostalCode)]
        [RegularExpression(@"^\d{3}\s?\d{2}$", ErrorMessage = "Postnummret är ogiltigt")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "Ange telefonummer")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\+\d{2}-\d{2}-\d{3}\s?\d{2}\s?\d{2}|\d{2,3}-?\d{3}\s?\d{2}\s?\d{2}$", ErrorMessage = "Telefonnumret är ogiltigt")]
        public string PhoneNumber { get; set; }
    }
}
