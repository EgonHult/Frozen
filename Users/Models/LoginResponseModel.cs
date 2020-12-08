using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Users.Models
{
    public class LoginResponseModel
    {
        public UserModel User { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
