using Microsoft.VisualStudio.TestTools.UnitTesting;
using Payments.Models;
using Payments.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.UnitTests
{
    [TestClass]
    public class PaymentsTest
    {
        [TestMethod]
        public void CreateAllPaymentTypes_CreatesThreePaymentTypes_ReturnOk()
        {
            // Arrange

            var paymentRepository = new PaymentRepository();

            // Act

            var payments = paymentRepository.CreateAllTypesOfPayments();

            // Assert

            Assert.AreEqual(payments[0].Id, 1);
            Assert.AreEqual(payments[1].Id, 2);
            Assert.AreEqual(payments[2].Id, 3);
        }

        [TestMethod]
        public void CreateAllPaymentTypes_CreatesThreePaymentTypes_ReturnNull()
        {
            // Arrange

            var paymentRepository = new PaymentRepository();

            // Act

            List<Models.PaymentModel> payments = null;

            // Assert

            Assert.AreEqual(payments, null);
        }

        [TestMethod]
        public void VerifyPayment_CreatesInstanceOfVerifiedPaymentType_ReturnsTrue()
        {
            // Arrange

            var paymentRepository = new PaymentRepository();
            var cardPayment = new CardModel
            {
                Id = 1,
                Type = "Bankkort",
                Number = 1234123412341234,
                ExpiryDate = DateTime.Now.AddYears(3),
                CVV = 123
            };

            // Act

            var verifiedPayment = paymentRepository.VerifyPayment(cardPayment.Id, cardPayment);

            // Assert

            Assert.IsTrue(verifiedPayment);
        }

        [TestMethod]
        public void VerifyPayment_CreatesInstanceOfUnverifiedPaymentType_ReturnsFalse()
        {
            // Arrange

            var paymentRepository = new PaymentRepository();
            var cardPayment = new CardModel
            {
                Id = 1,
                Type = "Bankkort",
                Number = 12341234,
                ExpiryDate = DateTime.Now.AddYears(3),
                CVV = 123
            };

            // Act

            var verifiedPayment = paymentRepository.VerifyPayment(cardPayment.Id, cardPayment);

            // Assert

            Assert.IsFalse(verifiedPayment);
        }
    }
}
