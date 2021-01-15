using System.Collections.Generic;

namespace Payments.Models
{
    public static class InternetBanks
    {
        public static List<string> AvailableBanks() {
            List<string> Banks = new List<string> { 
                "Nordea", 
                "Handelsbanken", 
                "Swedbank", 
                "SEB", 
                "Avanza" };
            return Banks;
            }
    }
}
