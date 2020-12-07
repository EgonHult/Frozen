using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Models
{
    public class OrderProduct
    {
        [Key]
        public Guid ProductId { get; set; }

        public Guid OrderId { get; set; }

        public Order Order { get; set; }

        public int Amount { get; set; }
    }
}
