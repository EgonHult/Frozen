using System;
using System.Collections.Generic;

namespace Gateway.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public int StatusId { get; set; }

        public int PaymentId { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime Date { get; set; }

        public List<OrderProduct> OrderProduct { get; set; }
    }
}
