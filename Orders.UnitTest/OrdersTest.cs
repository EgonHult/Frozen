using Microsoft.VisualStudio.TestTools.UnitTesting;
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
           
            DeleteDummyOrderFromDatabase(dummyOrder);
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
        public void CreateOrderAsync_TryCreateOrderWithEmptyModel_ReturnNull()
        {
            //Arrange
            var orderModel = new OrderModel();

            //Act
            var EmptyOrder = OrderRepository.CreateOrderAsync(orderModel).Result;

            //Assert
            Assert.IsNull(EmptyOrder);
        }

        [TestMethod]
        public void CreateOrderAsync_TryCreateOrderWithAllReadyExistingOrderIdInDatabase_ReturnNull()
        {          
            // Arrange               
            var dummyOrder = DummyTestOrder.TestOrder();
            var dummyOrder2 = dummyOrder;
              
            //Act 
            var newOrder = OrderRepository.CreateOrderAsync(dummyOrder).Result;
            var newOrder2 = OrderRepository.CreateOrderAsync(dummyOrder2).Result;

            //Assert
            Assert.AreEqual(newOrder2, null);
           
            DeleteDummyOrderFromDatabase(dummyOrder);
        }

        [TestMethod]
        public void DeleteOrderByOrderIdAsync_DeleteOrderFromDatabse_ReturnDeletedOrderAreEqual()
        {

            // Arrange
            OrderModel dummyOrder = CreateDummyOrderToDatabase();

            // Act               
            var deletedProduct = OrderRepository.DeleteOrderByOrderIdAsync(dummyOrder.Id).Result;

            // Assert
            Assert.AreEqual(dummyOrder, deletedProduct);                     
        }

        [TestMethod]
        public void DeleteOrderByOrderIdAsync_TryDeleteByEmptyId_ReturnNull()
        {                      
            //Arrange
            var emptyOrderId = Guid.Empty;

            //Act              
            var result = OrderRepository.DeleteOrderByOrderIdAsync(emptyOrderId).Result;

            //Assert
            Assert.IsNull(result);           
        }

        [TestMethod]
        public void DeleteOrderByOrderIdAsync_TryDeleteNonExistingOrder_ReturnNull()
        {                         
            // Arrange
            var nonExistingOrderId = Guid.NewGuid();

            // Act            
            var order = OrderRepository.DeleteOrderByOrderIdAsync(nonExistingOrderId).Result;

            // Arrange
            Assert.IsNull(order);                          
        }

        [TestMethod]
        public void GetOrderByOrderIdAsync_GetOrderById_ReturnOrder()
        {
            // Arrange 
            OrderModel dummyOrder = CreateDummyOrderToDatabase();

            // Act            
            var order = OrderRepository.GetOrderByOrderIdAsync(dummyOrder.Id).Result;

            // Assert
            Assert.IsNotNull(order);
            
            DeleteDummyOrderFromDatabase(dummyOrder);
        }
    
        [TestMethod]
        public void GetOrderByOrderIdAsync_GetNonExistingOrder_ReturnNull()
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
            OrderModel dummyOrder = CreateDummyOrderToDatabase();
            var oldStatusId = 1; 
            dummyOrder.StatusId = 2;

            // Act
            var order = OrderRepository.UpdateOrderAsync(dummyOrder).Result;

            // Assert
            Assert.AreNotEqual(oldStatusId, order.StatusId);
            Assert.AreEqual(dummyOrder.StatusId, order.StatusId);
            
            DeleteDummyOrderFromDatabase(dummyOrder);
        }

        [TestMethod]
        public void UpdateOrderAsync_TryUpdateNullOrder_ReturnNull()
        {
            // Arrange
            OrderModel dummyOrder = null;

            // Act            
            var order = OrderRepository.UpdateOrderAsync(dummyOrder).Result;

            // Assert
            Assert.IsNull(order);
        }

        [TestMethod]
        public void UpdateOrderAsync_TryUpdateNotExistingOrder_ReturnNull()
        {
            // Arrange
            var dummyOrder = DummyTestOrder.TestOrder();

            // Act            
            var order = OrderRepository.UpdateOrderAsync(dummyOrder).Result;

            // Assert
            Assert.IsNull(order);
        }

        [TestMethod]
        public void GetOrdersByUserId_GetAllOrdersFromTheSpecificUserId_ReturnAllOrdersWithTheUserId()
        {

            // Arrange 
            OrderModel dummyOrder = CreateDummyOrderToDatabase();

            // Act              
            var orders = OrderRepository.GetOrdersByUserIdAsync(dummyOrder.UserId).Result;

            // Assert
            Assert.IsInstanceOfType(orders, typeof(List<OrderModel>));
            
            DeleteDummyOrderFromDatabase(dummyOrder);
        }

        [TestMethod]
        public void GetOrdersByUserId_GetNonExistingOrdersFromTheSpecificUserId_ReturnNull()
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
            OrderModel dummyOrder = CreateDummyOrderToDatabase();

            // Act              
            var result = OrderRepository.UpdateOrderStatusAsync(2, dummyOrder.Id).Result;

            // Assert
            Assert.IsTrue(result);
           
            DeleteDummyOrderFromDatabase(dummyOrder);
        }

        [TestMethod]
        public void UpdateOrderStatusAsync_UpdateOrderStatusToNonExistingStatus_ReturnFalse()
        {
            // Arrange 
            OrderModel dummyOrder = CreateDummyOrderToDatabase();

            // Act             
            var result = OrderRepository.UpdateOrderStatusAsync(10, dummyOrder.Id).Result;

            // Assert
            Assert.IsFalse(result);
            
            DeleteDummyOrderFromDatabase(dummyOrder);
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



        private static OrderModel CreateDummyOrderToDatabase()
        {
            var dummyOrder = DummyTestOrder.TestOrder();
            OrderTestContext.DbContext.Order.Add(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();
            return dummyOrder;
        }

        private static void DeleteDummyOrderFromDatabase(OrderModel dummyOrder)
        {           
            OrderTestContext.DbContext.Remove(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();
        }

    }
}
