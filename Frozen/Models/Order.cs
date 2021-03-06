﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frozen.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public int StatusId { get; set; }

        public OrderStatus Status { get; set; }

        public int PaymentId { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime Date { get; set; }

        public List<OrderProduct> OrderProduct { get; set; }
    }

}
