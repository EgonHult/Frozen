﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Orders.Models;
using Orders.Repositories;
using Orders.UnitTest.Context;
using Orders.UnitTest.DummyOrder;
using System.Collections.Generic;

namespace Orders.UnitTest
{
    [TestClass]
   public class OrdersTest
    {

        public  static IOrderRepository OrderRepository { get; set; }
        public static TestOrdersDbContext OrderTestContext { get; set; }



        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            OrderTestContext = new TestOrdersDbContext();
            OrderRepository = new OrderRepository(OrderTestContext.DbContext);
        }

        [TestMethod]
        public void CreateOrderAsync_CreateNewOrder_ReturnCreatedOrder()
        {           
            // Arrange           
            var dummyOrder = DummyTestOrder.TestOrder();

            // Act
            var newOrder = OrderRepository.CreateOrderAsync(dummyOrder).Result;

            // Assert
            Assert.AreEqual(dummyOrder, newOrder);

            // Delete dummyOrder from DB
            OrderTestContext.DbContext.Remove(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();

        }

        [TestMethod]
        public void CreateOrderAsync_CreateOrderWithoutOrderProducts_ReturnNull()
        {           
            //Arrange              
            var dummyOrder = DummyTestOrder.TestOrderWithoutOrderProduct();

            //Act
            var result = OrderRepository.CreateOrderAsync(dummyOrder).Result;

            //Assert
            Assert.IsNull(result);            
        }

        [TestMethod]
        public void CreateOrderAsync_TryCreateNullOrder_ReturnNull()
        {         
            // Arrange                       
            OrderModel dummyOrder = null;
              
            //Act 
            var nullOrder = OrderRepository.CreateOrderAsync(dummyOrder).Result;

            //Assert
            Assert.IsNull(nullOrder);            
        }

        [TestMethod]
        public void CreateOrderAsync_TryGetOrderWithEmptyModel_ReturnNull()
        {
            //Arrange
            var orderModel = new OrderModel();

            //Act
            var EmptyOrder = OrderRepository.CreateOrderAsync(orderModel).Result;

            //Assert
            Assert.IsNull(EmptyOrder);
        }

        [TestMethod]
        public void CreateOrderAsync_TryCreateOrderWithExistingOrderId_ReturnNull()
        {
           
            // Arrange               
            var dummyOrder = DummyTestOrder.TestOrder();
            var dummyOrder2 = dummyOrder;
              
            //Act 

            var newOrder = OrderRepository.CreateOrderAsync(dummyOrder).Result;
            var newOrder2 = OrderRepository.CreateOrderAsync(dummyOrder2).Result;

            //Assert
            Assert.AreEqual(newOrder2, null);

            // Delete dummyOrder from DB
            OrderTestContext.DbContext.Remove(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();
            
        }

        [TestMethod]
        public void DeleteOrderByIdAsync_DeleteOrderFromDatabse_ReturnDeletedOrderAreEqual()
        {
           
            // Arrange
            var dummyOrder = DummyTestOrder.TestOrder();
            OrderTestContext.DbContext.Order.Add(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();

            // Act               
            var deletedProduct = OrderRepository.DeleteOrderByOrderIdAsync(dummyOrder.Id).Result;

            // Assert
            Assert.AreEqual(dummyOrder, deletedProduct);         
            
        }

        [TestMethod]
        public void DeleteOrderByIdAsync_TryDeleteByEmtyId_ReturnNull()
        {                      
            //Arrange
            var emtyOrderId = Guid.Empty;

            //Act              
            var result = OrderRepository.DeleteOrderByOrderIdAsync(emtyOrderId).Result;

            //Assert
            Assert.IsNull(result);           
        }

        [TestMethod]
        public void DeleteOrderByIdAsync_TryDeleteNonExistingOrder_ReturnNull()
    {                         
            // Arrange
            var nonExistingOrderId = Guid.NewGuid();

            // Act            
            var order = OrderRepository.DeleteOrderByOrderIdAsync(nonExistingOrderId).Result;

            // Arrange
            Assert.IsNull(order);                          
        }

        [TestMethod]
        public void GetOrderByIdAsync_GetOrderById_ReturnOrder()
        {          
            // Arrange 
            var dummyOrder = DummyTestOrder.TestOrder();
            OrderTestContext.DbContext.Order.Add(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();

            // Act            
            var order = OrderRepository.GetOrderByOrderIdAsync(dummyOrder.Id).Result;

            // Assert
            Assert.IsNotNull(order);

            // Delete dummyOrder from DB
            OrderTestContext.DbContext.Remove(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();            
        }
    
        [TestMethod]
        public void GetOrderByIdAsync_GetNonExistingOrder_ReturnNull()
        {
          
            // Arrange 
            var dummyOrder = DummyTestOrder.TestOrder();
              
            // Act              
            var order = OrderRepository.GetOrderByOrderIdAsync(dummyOrder.Id).Result;

            // Assert
            Assert.IsNull(order);
            
        }

        [TestMethod]
        public void GetAllOrdersAsync_GetAllOrdersFromDatbase_ReturnListOfOrders()
        {                       
            // Act             
            var orders = OrderRepository.GetAllOrdersAsync().Result;

            // Assert
            Assert.IsInstanceOfType(orders, typeof(List<OrderModel>));                     
        }

        [TestMethod]
        public void UpdateOrderAsync_UpdateStatus_ReturnUpdatedOrder()
        {
           
            // Arrange
            var dummyOrder = DummyTestOrder.TestOrder();
            OrderTestContext.DbContext.Order.Add(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();
            var oldStatusId = 1; 
            dummyOrder.StatusId = 2;

            // Act
            var order = OrderRepository.UpdateOrderAsync(dummyOrder).Result;

            // Assert
            Assert.AreNotEqual(oldStatusId, order.StatusId);
            Assert.AreEqual(dummyOrder.StatusId, order.StatusId);

            // Delete dummyOrder from DB
            OrderTestContext.DbContext.Remove(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();
            
        }

        [TestMethod]
        public void GetOrdersByUserId_GetAllOrdersFromTheSpecsificUserId_ReturnAllOrdersWithTheUserId()
        {
         
            // Arrange 
            var dummyOrder = DummyTestOrder.TestOrder();
            OrderTestContext.DbContext.Order.Add(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();

            // Act              
            var orders = OrderRepository.GetOrdersByUserIdAsync(dummyOrder.UserId).Result;

            // Assert
            Assert.IsInstanceOfType(orders, typeof(List<OrderModel>));

            // Delete dummyOrder from DB
            OrderTestContext.DbContext.Remove(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();
            
        }

        [TestMethod]
        public void GetOrdersByUserId_GetNonExistingOrdersFromTheSpecsificUserId_ReturnNull()
        {        
            // Arrange 
            var dummyOrder = DummyTestOrder.TestOrder();

            // Act             
            var orders = OrderRepository.GetOrdersByUserIdAsync(dummyOrder.UserId).Result;

            // Assert
            Assert.IsNull(orders);
        }
      
        [TestMethod]
        public void UpdateOrderStatusAsync_UpdateOrderStatusToSent_ReturnTrue()
        {          
            // Arrange 
            var dummyOrder = DummyTestOrder.TestOrder();
            OrderTestContext.DbContext.Order.Add(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();

            // Act              
            var result = OrderRepository.UpdateOrderStatusAsync(2, dummyOrder.Id).Result;

            // Assert
            Assert.IsTrue(result);

            // Delete dummyOrder from DB
            OrderTestContext.DbContext.Remove(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();         
        }

        [TestMethod]
        public void UpdateOrderStatusAsync_UpdateOrderStatusToNonExistingStatus_ReturnFalse()
        {          
            // Arrange 
            var dummyOrder = DummyTestOrder.TestOrder();
            OrderTestContext.DbContext.Order.Add(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();

            // Act             
            var result = OrderRepository.UpdateOrderStatusAsync(10, dummyOrder.Id).Result;

            // Assert
            Assert.IsFalse(result);

            // Delete dummyOrder from DB
            OrderTestContext.DbContext.Remove(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();          
        }

        [TestMethod]
        public void UpdateOrderStatusAsync_TryUpdateStatusOfNonExistingOrder_ReturnFalse()
        {
          
            // Arrange 
            var nonExistingOrderId = Guid.NewGuid();

            // Act           
            var result = OrderRepository.UpdateOrderStatusAsync(3, nonExistingOrderId).Result;

            // Assert
            Assert.IsFalse(result);
           
        }

    }
}
