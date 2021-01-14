using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frozen.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
    public class CardModel : Payment
    {
        public long Number { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int CVV { get; set; }
    }
    public class InternetBankModel : Payment
    {
        public string Bank { get; set; }
    }
    public static class InternetBanks
    {
        public static List<string> AvailableBanks()
        {
            List<string> Banks = new List<string> {
                "Nordea",
                "Handelsbanken",
                "Swebank",
                "SEB",
                "Avanza" };
            return Banks;
        }
    }
    public class SwishModel : Payment
    {
        public string PhoneNumber { get; set; }
    }
}
