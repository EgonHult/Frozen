using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frozen.Models
{
    public class LoggedInUser
    {
        public User User { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
