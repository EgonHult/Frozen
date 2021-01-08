using System.Collections.Generic;

namespace Payments.Models
{
    public static class InternetBanks
    {
        public static List<string> AvailableBanks() {
            List<string> Banks = new List<string> { 
                "Nordea", 
                "Handelsbanken", 
                "Swebank", 
                "SEB", 
                "Avanza" };
            return Banks;
            }
    }
}
