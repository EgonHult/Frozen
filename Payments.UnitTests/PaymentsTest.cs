using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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
        public void VerifyPayment_VerifySwishPayment_ReturnTrue()
        {

            // Arrange
            var paymentRepository = new PaymentRepository();
            var swishPayment = new SwishModel
            {
                Id = 2,
                PhoneNumber = "0701234567"
            };

            // Act
            var verifiedPayment = paymentRepository.VerifyPayment(swishPayment.Id, swishPayment.PhoneNumber);

            // Assert
            Assert.IsTrue(verifiedPayment);
        }

        [TestMethod]
        public void VerifyPayment_VerifyIncompleteSwishPayment_ReturnFalse()
        {

            // Arrange
            var paymentRepository = new PaymentRepository();
            var swishPayment = new SwishModel
            {
                Id = 2,
                PhoneNumber = "070"
            };

            // Act
            var verifiedPayment = paymentRepository.VerifyPayment(swishPayment.Id, swishPayment.PhoneNumber);

            // Assert
            Assert.IsFalse(verifiedPayment);
        }

        [TestMethod]
        public void VerifyPayment_VerifyInternetBankPayment_ReturnTrue()
        {

            // Arrange
            var paymentRepository = new PaymentRepository();
            var internetBankPayment = new InternetBankModel
            {
                Id = 3,
                Type = "Internetbank",
                Bank = "Nordea"
            };

            // Act
            var verifiedPayment = paymentRepository.VerifyPayment(internetBankPayment.Id, internetBankPayment.Bank);

            // Assert
            Assert.IsTrue(verifiedPayment);
        }

        [TestMethod]
        public void VerifyPayment_VerifyIncompleteInternetBankPayment_ReturnFalse()
        {

            // Arrange
            var paymentRepository = new PaymentRepository();
            var internetBankPayment = new InternetBankModel
            {
                Id = 3,
                Type = "Internetbank",
                Bank = "Danskebank"
            };

            // Act
            var verifiedPayment = paymentRepository.VerifyPayment(internetBankPayment.Id, internetBankPayment.Bank);

            // Assert
            Assert.IsFalse(verifiedPayment);
        }

        [TestMethod]
        public void VerifyPayment_VerifyCardBankPayment_ReturnTrue()
        {

            // Arrange
            var paymentRepository = new PaymentRepository();
            var cardPayment = new CardModel
            {
                Id = 1,
                Type = "Bankkort",
                CardOwner = "Big Mac",
                Number = 1234123412341234,
                ExpiryDate = DateTime.Now.AddYears(3),
                CVV = 123
            };

            var cardJsonObject = JsonConvert.SerializeObject(cardPayment);

            // Act
            var verifiedPayment = paymentRepository.VerifyPayment(cardPayment.Id, cardJsonObject);

            // Assert
            Assert.IsTrue(verifiedPayment);
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
                CardOwner = "Kalle Anka",
                Number = 12341234,
                ExpiryDate = DateTime.Now.AddYears(3),
                CVV = 123
            };

            var cardJsonObject = JsonConvert.SerializeObject(cardPayment);

            // Act
            var verifiedPayment = paymentRepository.VerifyPayment(cardPayment.Id, cardJsonObject);

            // Assert
            Assert.IsFalse(verifiedPayment);
        }
    }
}
