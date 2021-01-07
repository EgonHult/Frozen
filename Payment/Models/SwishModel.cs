using System.ComponentModel.DataAnnotations;

namespace Payments.Models
{
    public class SwishModel : PaymentModel
    {
        [Required]
        [RegularExpression(@"\d{10,13}")]
        public int PhoneNumber { get; set; }
    }
}
