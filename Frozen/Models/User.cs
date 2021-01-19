using System;
using System.ComponentModel.DataAnnotations;

namespace Frozen.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Ange förnamn")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Ange efternamn")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Ange emailadress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ange ett telefonnummer")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Ange adress")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Ange stad")]
        public string City { get; set; }

        [Required(ErrorMessage = "Ange postnummer")]
        public string Zip { get; set; }
    }
}
