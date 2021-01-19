using System;
using System.ComponentModel.DataAnnotations;

namespace Frozen.Models
{
    public class OrderProduct
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid OrderModelId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

}
