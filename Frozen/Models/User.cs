using System;
using System.ComponentModel.DataAnnotations;

namespace Frozen.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Ange förnamn")]
        [RegularExpression(@"^\p{L}+[ \s]?\p{L}+$", ErrorMessage = "Ogiltigt förnamn")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Ange efternamn")]
        [RegularExpression(@"^\p{L}+[ \s]?\p{L}+$", ErrorMessage = "Ogiltigt efternamn")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Ange emailadress")]
        [RegularExpression(@"^[a-z\d._%+-]+@[a-z\d.-]+\.[a-z]{2,}$", ErrorMessage = "E-postadressen är ogiltig")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ange ett telefonnummer")]
        [RegularExpression(@"^\+\d{2}-\d{2}-\d{3}\s?\d{2}\s?\d{2}|\d{2,3}-?\d{3}\s?\d{2}\s?\d{2}$", ErrorMessage = "Telefonnumret är ogiltigt")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Ange adress")]
        [RegularExpression(@"^\p{L}{4,} \d+$", ErrorMessage = "Ogiltig adress")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Ange stad")]
        [RegularExpression(@"^\p{L}+([ \s]\p{L}+)?$", ErrorMessage = "Ogiltig stad")]
        public string City { get; set; }

        [Required(ErrorMessage = "Ange postnummer")]
        [RegularExpression(@"^\d{3}\s?\d{2}$", ErrorMessage = "Postnummret är ogiltigt")]
        public string Zip { get; set; }
    }
}
