using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Frozen.Models
{
    public class OrderResponse
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


    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class OrderProduct
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

}
