using Payments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.Repositories
{
    public interface IPaymentRepository
    {
        List<PaymentModel> CreateAllTypesOfPayments();
        bool VerifyPayment(int id, object paymentType);
    }
}
