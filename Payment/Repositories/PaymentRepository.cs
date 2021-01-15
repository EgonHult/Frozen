using Newtonsoft.Json;
using Payments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        public List<PaymentModel> CreateAllTypesOfPayments()
        {
            List<PaymentModel> paymentTypes = new List<PaymentModel>();

            PaymentModel payment1 = new PaymentModel()
            {
                Id = 1,
                Type = "Bankkort"
            };

            PaymentModel payment2 = new PaymentModel()
            {
                Id = 2,
                Type = "Swish"
            };

            PaymentModel payment3 = new PaymentModel()
            {
                Id = 3,
                Type = "Internetbank"
            };

            paymentTypes.Add(payment1);
            paymentTypes.Add(payment2);
            paymentTypes.Add(payment3);

            return paymentTypes;
        }

        public bool VerifyPayment(int id, object payment)
        {

            switch(id)
            {
                case 1:
                    return VerifyCard(payment);
                case 2:
                    return VerifySwish(payment);
                case 3:
                    return VerifyInternetBank(payment);
                default:
                    return false;
            }
        }

        private static bool VerifyInternetBank(object payment)
        {
            string internetBankPayment = payment.ToString();
            if (InternetBanks.AvailableBanks().Any(x => x == internetBankPayment))
            {
                return true;
            }
            return false;
        }

        private static bool VerifySwish(object payment)
        {
            string swishPayment = payment.ToString();
            if (swishPayment.Length == 10 ||
                swishPayment.Length == 13)
            {
                return true;
            }
            return false;
        }

        private static bool VerifyCard(object payment)
        {
            var cardPayment = JsonConvert.DeserializeObject<CardModel>(payment.ToString());

            var cardExpire = ParseDateToYearMonthInt(cardPayment.ExpiryDate);
            var now = ParseDateToYearMonthInt(DateTime.Now);

            if (cardExpire >= now)
                return true;
            else
                return false;
        }

        private static int ParseDateToYearMonthInt(DateTime date)
        {
            return int.Parse(date.ToString("yyyyM"));
        }
    }
}
