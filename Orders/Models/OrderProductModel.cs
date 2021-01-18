﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Orders.Models
{
    public class OrderProductModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid OrderModelId { get; set; }
        public string Name { get; set; }       
        public int Quantity { get; set; }
    }
}
