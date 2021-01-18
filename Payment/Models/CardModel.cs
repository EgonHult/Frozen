using System;
using System.ComponentModel.DataAnnotations;

namespace Payments.Models
{
    public class CardModel : PaymentModel
    {
        [Required]
        [RegularExpression(@"^\p{L}{2,20}[-\s\p{L}]+\p{L}$")]
        public string CardOwner { get; set; }

        [Required]
        [RegularExpression(@"\d{16}")]
        public long Number { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [RegularExpression(@"\d{3}")]
        public int CVV { get; set; }
    }
}
