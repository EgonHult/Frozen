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
    }
}
