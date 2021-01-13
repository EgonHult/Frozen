using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Frozen.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        [DisplayName("Produkter")]
        public string Name { get; set; }

        [DisplayName("Pris")]
        public decimal Price { get; set; }
        public string Details { get; set; }    
        public int Quantity { get; set; }
        public string Image { get; set; }
        public int WeightInGrams { get; set; }
    }
}
