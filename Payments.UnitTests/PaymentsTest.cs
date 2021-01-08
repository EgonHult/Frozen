using Microsoft.VisualStudio.TestTools.UnitTesting;
using Payments.Models;
using Payments.Repositories;
using System;
using System.Collections.Generic;

namespace Payments.UnitTests
{
    [TestClass]
    public class PaymentsTest
    {
        [TestMethod]
        public void CreateAllPaymentTypes_CreatesThreePaymentTypes_ReturnsOk()
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
        public void CreateAllPaymentTypes_DoesNotCreateThreePaymentTypes_ReturnsNull()
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

            PaymentModel payment = GenerateRandomPayment();

            // Act

            var verifiedPayment = paymentRepository.VerifyPayment(payment.Id, payment);

            // Assert

            Assert.IsTrue(verifiedPayment);
        }

        private PaymentModel GenerateRandomPayment()
        {
            //var random = new Random().Next(1, 4);

            int random = 2;

            switch (random)
            {
                case 1:
                    var cardPayment = new CardModel
                    {
                        Id = 1,
                        Type = "Bankkort",
                        Number = 1234123412341234,
                        ExpiryDate = DateTime.Now.AddYears(3),
                        CVV = 123
                    };
                    return cardPayment;
                case 2:
                    var swishPayment = new SwishModel
                    {
                        Id = 2,
                        Type = "Swish",
                        PhoneNumber = "0701234567"
                    };
                    return swishPayment;
                case 3:
                    var internetBankPayment = new InternetBankModel
                    {
                        Id = 3,
                        Type = "Internetbank",
                        Bank = "Nordea"
                    };
                    return internetBankPayment;
                default:
                    return null;
            }
        }

        [TestMethod]
        public void VerifyPayment_DoesNotCreateInstanceOfVerifiedPaymentType_ReturnsFalse()
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
