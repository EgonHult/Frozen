using System.ComponentModel.DataAnnotations;

namespace Payments.Models
{
    public class SwishModel : PaymentModel
    {
        [Required]
        public string PhoneNumber { get; set; }
    }
}
