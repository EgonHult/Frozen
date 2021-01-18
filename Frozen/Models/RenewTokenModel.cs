using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frozen.Models
{
    public class RenewTokenModel
    {
        public Guid UserID { get; set; }
        public string Token { get; set; }
    }
}
