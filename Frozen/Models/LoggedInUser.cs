using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frozen.Models
{
    public class LoggedInUser : TokenModel
    {
        public User User { get; set; }
    }
}
