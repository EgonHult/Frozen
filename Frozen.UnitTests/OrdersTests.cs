﻿using Frozen.Common;
using Frozen.Controllers;
using Frozen.Models;
using Frozen.Services;
using Frozen.UnitTests.DummyData;
using Frozen.UnitTests.Sessions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frozen.UnitTests
{
    [TestClass]
    public class OrdersTests
    {
        static OrderDummyData DummyData { get; set; }
        static OrderModel DummyOrder { get; set; }
        static string JwtToken { get; set; }
        static OrderController Controller { get; set; }


        [ClassInitialize]
        public static void TestFixtureSetup(TestContext Context)
        {
            DummyData = new OrderDummyData();
            DummyOrder = DummyData.CreateDummyOrder();

            // Initialize HttpContext for accessing session functionality
            var config = new HttpContextConfig();
            IHttpContextAccessor httpContext = config.HttpContext;
            ICookieHandler _cookieHandler = new CookieHandler(httpContext);
            IClientService clientService = new ClientService(_cookieHandler);

            CartService cartService = new CartService(httpContext);

            // Mock a session cookie with JwtToken
            JwtToken = AdminToken.LoginAdmin(clientService);
            _cookieHandler.CreateSessionCookie(Cookies.JWT_SESSION_TOKEN, JwtToken);

            // Arrange for all tests
            Controller = new OrderController(clientService, cartService, _cookieHandler);
        }

        [ClassCleanup]
        public static void TestFixtureDispose()
        {
            DummyData.DeleteDummyOrder(DummyOrder.Id);
        }

        [TestMethod]
        public void GetOrdersAsync_GetAllOrders_ReturnAllOrders()
        {
            //Act
            var orders = Controller.GetOrdersAsync().Result;

            //Assert
            Assert.IsInstanceOfType(orders, typeof(List<Order>));
            Assert.IsTrue(orders.Count > 0);
        }

        [TestMethod]
        public void GetOrderByIdAsync_GetOrder_ReturnOrder()
        {
            //Arrange
            var id = DummyOrder.Id;
            //Act
            var order = Controller.GetOrderByIdAsync(id).Result;

            //Assert
            Assert.AreEqual(order.Id, id);
        }

        [TestMethod]
        public void GetOrderByIdAsync_TryGetOrderWithNonExistingId_ReturnNull()
        {
            var id = Guid.NewGuid();

            var result = Controller.GetOrderByIdAsync(id).Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void PostOrderAsync_CreateNewOrder_ReturnCreatedOrder()
        {
            //Arrange
            Order order = new Order
            {
                StatusId = 2,
                TotalPrice = 420M,
                PaymentId = 1,
                UserId = Guid.NewGuid(),
                Date = DateTime.Now,

                OrderProduct = new List<OrderProduct>()
                {
                    new OrderProduct { Quantity = 2, ProductId = Guid.NewGuid(), Name = "Testglass" },
                    new OrderProduct { Quantity = 2, ProductId = Guid.NewGuid(), Name = "Provglass" }
                }
            };
            //Act
            var result = Controller.PostOrderAsync(order).Result;

            //Assert
            Assert.AreEqual(result.UserId, order.UserId);
            DummyData.DeleteDummyOrder(result.Id);
        }

        [TestMethod]
        public void PostOrderAsync_TryCreateNewOrderWithEmptyOrderModel_ReturnNull()
        {
            Order dummyOrder = new Order();

            var result = Controller.PostOrderAsync(dummyOrder).Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void PostOrderAsync_TryCreateNewOrderWithNullOrder_ReturnNull()
        {
            Order dummyOrder = null;

            var result = Controller.PostOrderAsync(dummyOrder).Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void UpdateOrderAsync_UpdateOrder_ReturnUpdatedOrder()
        {
            //Arrange
            Order order = new Order
            {
                Id = DummyOrder.Id,
                TotalPrice = 420M,
                StatusId = 2,
                PaymentId = 1,
                UserId = Guid.Parse("a5fa1093-aebc-49ac-8636-5c171c4d3991"),
                Date = DateTime.Now,
            };

            //Act
            var result = Controller.UpdateOrderAsync(DummyOrder.Id, order).Result;

            //Assert
            Assert.AreEqual(DummyOrder.Id, result.Id);
            Assert.AreEqual(result.TotalPrice, order.TotalPrice);
        }

        [TestMethod]

        public void UpdateOrderAsync_UpdateOrderWithNotExistingOrder_ReturnNull()
        {
            //Arrange            
            Order order = new Order
            {
                Id = Guid.NewGuid(),
                TotalPrice = 420M,
                StatusId = 2,
                PaymentId = 1,
                UserId = Guid.Parse("a5fa1093-aebc-49ac-8636-5c171c4d3991"),
                Date = DateTime.Now,
            };

            //Act
            var result = Controller.UpdateOrderAsync(DummyOrder.Id, order).Result;

            //Assert
            Assert.IsNull(result);            
        }

        [TestMethod]
        public void DeleteOrderAsync_DeleteOrder_ReturnDeletedOrder()
        {
            //Arrange
            var DummyOrderTwo = DummyData.CreateDummyOrder();
            var Id = DummyOrderTwo.Id;

            //Act
            var result = Controller.DeleteOrderAsync(Id).Result;

            //Assert
            Assert.AreEqual(result.Id, Id);
            if (result == null)
            {
                DummyData.DeleteDummyOrder(Id);
            }
        }

        [TestMethod]
        public void DeleteOrderAsync_DeleteOrderWithNonExistingOrderId_ReturnNull()
        {
            var orderId = Guid.NewGuid();

            var result = Controller.DeleteOrderAsync(orderId).Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetOrdersByUserIdAsync_GetUsersOrders_ReturnUsersOrders()
        {
            //Arrange
            var Id = DummyOrder.UserId;

            //Act
            var result = Controller.GetOrdersByUserIdAsync(Id).Result;

            //Assert
            Assert.AreEqual(result[0].UserId, Id);
        }

        [TestMethod]
        public void GetOrdersByUserIdAsync_TryGetOrderWithNonExistingUserId_ReturnNull()
        {
            //Arrange
            var userId = Guid.NewGuid();

            //Act
            var result = Controller.GetOrdersByUserIdAsync(userId).Result;

            //Assert
            Assert.IsNull(result);
        }
    }
}
