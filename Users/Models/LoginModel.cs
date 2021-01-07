using System.ComponentModel.DataAnnotations;

namespace Users.Models
{
    public class LoginModel
    {
        const string PASSWORD_ERR_MESSAGE = "Lösenordet måste bestå av minst 6 tecken. Innehålla stor och liten bostav med siffra och specialtecken.";
        const string USERNAME_ERR_MESSAGE = "Användarnamnet är epostadressen som användes vid registreringen av kontot";

        [Required]
        [EmailAddress(ErrorMessage = USERNAME_ERR_MESSAGE)]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W)[a-zA-Z\W\d]{6,}", ErrorMessage = PASSWORD_ERR_MESSAGE)]
        public string Password { get; set; }
    }
}
