using Frozen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frozen.ViewModels
{
    public class OrderViewModel
    {
        public List<CartItem> Cart { get; set; }
        public User User { get; set; }
        public List<Payment> PaymentMethods { get; set; }
    }
}
