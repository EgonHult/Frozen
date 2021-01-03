using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Models
{
    public class Order
    {     
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public int StatusId { get; set; }

        public Status Status { get; set; }

        public int PaymentId { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime Date { get; set; }

        public List<OrderProduct> OrderProduct { get; set; }
    }
}