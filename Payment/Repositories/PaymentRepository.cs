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
                    var cardPayment = (CardModel)payment;
                    if (cardPayment.ExpiryDate > DateTime.Now.AddMonths(1) &&
                        cardPayment.Number.ToString().Length == 16 &&
                        cardPayment.CVV.ToString().Length == 3)
                    {
                        return true;
                    }
                    return false;
                case 2:
                    var swishPayment = (SwishModel)payment;
                    if (swishPayment.PhoneNumber.ToString().Length == 10 ||
                        swishPayment.PhoneNumber.ToString().Length == 13)
                    {
                        return true;
                    }
                    return false;
                case 3:
                    var internetBankPayment = (InternetBankModel)payment;
                    if (InternetBanks.AvailableBanks().Any(x => x == internetBankPayment.Bank))
                    {
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

        //public bool CreateCardPayment()
    }
}
