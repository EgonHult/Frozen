using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payments.Models;
using Payments.Repositories;

namespace Payments.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentsController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [HttpGet]
        public List<PaymentModel> GetPayments()
        {
            var allPaymentTypes = _paymentRepository.CreateAllTypesOfPayments();
            return allPaymentTypes;
        }
    }
}
