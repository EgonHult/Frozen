using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Frozen.Models
{
    public class Product
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [DisplayName("Produkter")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Pris")]
        public decimal Price { get; set; }
        [Required]
        [DisplayName("Beskrivning")]
        public string Details { get; set; }  
        [Required]
        [DisplayName("Antal i lager")]
        public int Quantity { get; set; }
        [Required]
        [DisplayName("Bild")]
        public string Image { get; set; }
        [Required]
        [DisplayName("Vikt i gram")]
        public int WeightInGrams { get; set; }
    }
}
