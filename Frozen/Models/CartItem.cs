using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Frozen.Models
{
    public class CartItem
    {
        public Product Product { get; set; }

        [DisplayName("Antal")]
        public int Quantity { get; set; }
    }
}
