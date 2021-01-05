using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Users.Models
{
    public class RenewTokenModel
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}
