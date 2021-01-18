namespace Frozen.Common
{
    public class ApiLocation
    {
        public class Gateway
        {
            public const string CREATE_ORDER = "https://localhost:44350/api/aggregate/";
        }

        public class Users
        {
            public const string GATEWAY_BASEURL             = "https://localhost:44350/user/";
            public const string LOGIN_ENDPOINT              = GATEWAY_BASEURL + "login/";
            public const string REGISTER_ENDPOINT           = GATEWAY_BASEURL + "register/";
            public const string REQUEST_NEW_TOKEN_ENDPOINT  = GATEWAY_BASEURL + "token/";
            public const string GET_USER                    = GATEWAY_BASEURL + "getuserbyid/";
        }

        public class Products
        {
            public const string GATEWAY_BASEURL             = "https://localhost:44350/product/";
            public const string ALL_PRODUCTS                = GATEWAY_BASEURL + "getall/";
            public const string CREATE_PRODUCT              = GATEWAY_BASEURL + "create/";
        }

        public class Orders
        {
            public const string GATEWAY_BASEURL             = "https://localhost:44350/order/";
            public const string ALL_ORDERS                  = GATEWAY_BASEURL + "getall/";
            public const string CREATE_ORDER                = GATEWAY_BASEURL + "create/";
            public const string GET_ORDER_BY_USERID         = GATEWAY_BASEURL + "user/";
        }
        public class Payments
        {
            public const string GATEWAY_BASEURL             = "https://localhost:44350/payment/";
            public const string GET_PAYMENTS                = GATEWAY_BASEURL + "getpayments/";
            public const string VERIFY_PAYMENT              = GATEWAY_BASEURL + "verifypayment/";
        }
    }
}