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
        [Required(ErrorMessage = "Ange produktnamn")]
        [DisplayName("Produkter")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Ange ett pris")]
        [DisplayName("Pris")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Ange en beskrivning")]
        [DisplayName("Beskrivning")]
        public string Details { get; set; }  
        [Required(ErrorMessage = "Ange lagersaldo")]
        [DisplayName("Antal i lager")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Ange en bildadress")]
        [DisplayName("Bild")]
        public string Image { get; set; }
        [Required(ErrorMessage = "Ange vikt")]
        [DisplayName("Vikt i gram")]
        public int WeightInGrams { get; set; }
    }
}
