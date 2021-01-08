using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payments.Models;
using Payments.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Payments.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentsController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [Authorize]
        [HttpGet("getpayments")]
        public List<PaymentModel> GetPayments()
        {
            var allPaymentTypes = _paymentRepository.CreateAllTypesOfPayments();
            return allPaymentTypes;
        }

        [Authorize]
        [HttpPost("verifypayment")]
        public IActionResult VerifyPayment(int id, object payment)
        {
            if (id != 0 && payment != null)
            {
                var verifiedPayment = _paymentRepository.VerifyPayment(id, payment);
                if (verifiedPayment)
                {
                    return Ok(verifiedPayment);
                }

                return BadRequest();
            }
            return BadRequest();
        }
    }
}
