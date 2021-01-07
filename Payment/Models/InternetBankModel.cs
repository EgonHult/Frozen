using System.ComponentModel.DataAnnotations;

namespace Payments.Models
{
    public class InternetBankModel : PaymentModel
    {
        [Required]
        public string Bank { get; set; }
    }
}
