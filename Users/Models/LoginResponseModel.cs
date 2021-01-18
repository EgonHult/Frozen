namespace Users.Models
{
    public class LoginResponseModel : TokenModel
    {
        public UserModel User { get; set; }
    }
}