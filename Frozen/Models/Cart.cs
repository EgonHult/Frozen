using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frozen.Models
{
    public class Cart : Product
    {
        public List<Product> Products { get; set; }
    }
}
