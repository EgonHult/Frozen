namespace Frozen.Common
{
    public class ApiLocation
    {
        public class Users
        {
            public const string GATEWAY_BASEURL             = "https://localhost:44350/user/";
            public const string LOGIN_ENDPOINT              = GATEWAY_BASEURL + "login/";
            public const string REGISTER_ENDPOINT           = GATEWAY_BASEURL + "register/";
            public const string REQUEST_NEW_TOKEN_ENDPOINT  = GATEWAY_BASEURL + "token/";
        }
    }
}