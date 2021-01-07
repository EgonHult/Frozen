using System.ComponentModel.DataAnnotations;

namespace Frozen.ViewModels
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Vänligen ange e-postadress")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Felaktig e-postadress")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vänligen ange lösenord")]
        [RegularExpression(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W)[a-zA-Z\W\d]{6,}", ErrorMessage = "Minst 6 tecken. Stor och liten bokstav, siffra och specialtecken")]
        public string Password { get; set; }

        public bool Remember { get; set; } = false;
    }
}
