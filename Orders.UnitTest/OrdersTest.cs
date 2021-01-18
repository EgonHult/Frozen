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

        [TestMethod]
        public void CreateOrderAsync_CreateNewOrder_ReturnCreatedOrder()
        {
            using(var context = new TestOrdersDbContext().DbContext)
            {
                // Arrange
                var orderRepository = new OrderRepository(context);
                var dummyOrder = DummyTestOrder.TestOrder();


                // Act
                var newOrder = orderRepository.CreateOrderAsync(dummyOrder).Result;

                // Assert
                Assert.AreEqual(dummyOrder, newOrder);

                // Delete dummyOrder from DB
                context.Remove(dummyOrder);
                context.SaveChanges();


            }
        }

        [TestMethod]
        public void CreateOrderAsync_CreateOrderWithoutOrderProducts_ReturnNull()
        {
            using (var context = new TestOrdersDbContext().DbContext)
            {
                //Arrange
                var orderRepository = new OrderRepository(context);
                var dummyOrder = DummyTestOrder.TestOrderWithoutOrderProduct();

                //Act
                var result = orderRepository.CreateOrderAsync(dummyOrder).Result;

                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void CreateOrderAsync_TryCreateNullOrder_ReturnNull()
        {
            using(var context = new TestOrdersDbContext().DbContext)
            {
                // Arrange
                
                var orderRepository = new OrderRepository(context);
                OrderModel dummyOrder = null;
              
                //Act 
                var nullOrder = orderRepository.CreateOrderAsync(dummyOrder).Result;

                //Assert
                Assert.IsNull(nullOrder);
            }
        }

        [TestMethod]
        public void CreateOrderAsync_TryCreateOrderWithExistingOrderId_ReturnNull()
        {
            using (var context = new TestOrdersDbContext().DbContext)
            {
                // Arrange

                var orderRepository = new OrderRepository(context);
                var dummyOrder = DummyTestOrder.TestOrder();
                var dummyOrder2 = dummyOrder;
              
                //Act 

                var newOrder = orderRepository.CreateOrderAsync(dummyOrder).Result;
                var newOrder2 = orderRepository.CreateOrderAsync(dummyOrder2).Result;

                //Assert
                Assert.AreEqual(newOrder2, null);

                // Delete dummyOrder from DB
                context.Remove(dummyOrder);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void DeleteOrderByIdAsync_DeleteOrderFromDatabse_ReturnDeletedOrderAreEqual()
        {
            using (var context = new TestOrdersDbContext().DbContext)
            {
                // Arrange
                var dummyOrder = DummyTestOrder.TestOrder();
                context.Order.Add(dummyOrder);
                context.SaveChanges();

                // Act
                var orderRepository = new OrderRepository(context);
                var deletedProduct = orderRepository.DeleteOrderByOrderIdAsync(dummyOrder.Id).Result;

                // Assert
                Assert.AreEqual(dummyOrder, deletedProduct);         
            }
        }

        [TestMethod]
        public void DeleteOrderByIdAsync_TryDeleteByEmtyId_ReturnNull()
        {
            using (var context = new TestOrdersDbContext().DbContext)
            {
                //Arrange
                var emtyOrderId = Guid.Empty;

                //Act
                var orderRepository  = new OrderRepository(context);
                var result = orderRepository.DeleteOrderByOrderIdAsync(emtyOrderId).Result;

                //Assert
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void DeleteOrderByIdAsync_TryDeleteNonExistingOrder_ReturnNull()
        {
            using (var context = new TestOrdersDbContext().DbContext)
            {
                
                // Arrange
                var nonExistingOrderId = Guid.NewGuid();

                // Act
                var orderRepository = new OrderRepository(context);
                var order = orderRepository.DeleteOrderByOrderIdAsync(nonExistingOrderId).Result;

                // Arrange
                Assert.IsNull(order);               
            }
        }

        [TestMethod]
        public void GetOrderByIdAsync_GetOrderById_ReturnOrder()
        {
            using (var context = new TestOrdersDbContext().DbContext)
            {
                // Arrange 
                var dummyOrder = DummyTestOrder.TestOrder();
                context.Order.Add(dummyOrder);
                context.SaveChanges();

                // Act
                var orderRepository = new OrderRepository(context);
                var order = orderRepository.GetOrderByOrderIdAsync(dummyOrder.Id).Result;

                // Assert
                Assert.IsNotNull(order);

                // Delete dummyOrder from DB
                context.Remove(dummyOrder);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void GetOrderByIdAsync_GetNonExistingOrder_ReturnNull()
        {
            using (var context = new TestOrdersDbContext().DbContext)
            {
                // Arrange 
                var dummyOrder = DummyTestOrder.TestOrder();
              
                // Act
                var orderRepository = new OrderRepository(context);
                var order = orderRepository.GetOrderByOrderIdAsync(dummyOrder.Id).Result;

                // Assert
                Assert.IsNull(order);
            }
        }

        [TestMethod]
        public void GetAllOrdersAsync_GetAllOrdersFromDatbase_ReturnListOfOrders()
        {
            using (var context = new TestOrdersDbContext().DbContext)
            {             

                // Act
                var orderrepository = new OrderRepository(context);
                var orders = orderrepository.GetAllOrdersAsync().Result;

                // Assert
                Assert.IsInstanceOfType(orders, typeof(List<OrderModel>));         
            }
        }

        [TestMethod]
        public void UpdateOrderAsync_UpdateStatus_ReturnUpdatedOrder()
        {
            using (var context = new TestOrdersDbContext().DbContext)
            {
                // Arrange
                var dummyOrder = DummyTestOrder.TestOrder();              
                context.Order.Add(dummyOrder);
                context.SaveChanges();
                var oldStatusId = 1; 

                // Act
                dummyOrder.StatusId = 2;
                var orderrepository = new OrderRepository(context);
                var order = orderrepository.UpdateOrderAsync(dummyOrder).Result;

                // Assert
                Assert.AreNotEqual(oldStatusId, order.StatusId);
                Assert.AreEqual(dummyOrder.StatusId, order.StatusId);

                // Delete dummyOrder from DB
                context.Remove(dummyOrder);         
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void GetOrdersByUserId_GetAllOrdersFromTheSpecsificUserId_ReturnAllOrdersWithTheUserId()
        {
            using (var context = new TestOrdersDbContext().DbContext)
            {
                // Arrange 
                var dummyOrder = DummyTestOrder.TestOrder();
                context.Order.Add(dummyOrder);
                context.SaveChanges();

                // Act
                var orderRepository = new OrderRepository(context);
                var orders = orderRepository.GetOrdersByUserIdAsync(dummyOrder.UserId).Result;

                // Assert
                Assert.IsInstanceOfType(orders, typeof(List<OrderModel>));

                // Delete dummyOrder from DB
                context.Remove(dummyOrder);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void GetOrdersByUserId_GetNonExistingOrdersFromTheSpecsificUserId_ReturnNull()
        {
            using (var context = new TestOrdersDbContext().DbContext)
            {
                // Arrange 
                var dummyOrder = DummyTestOrder.TestOrder();


                // Act
                var orderRepository = new OrderRepository(context);
                var orders = orderRepository.GetOrdersByUserIdAsync(dummyOrder.UserId).Result;

                // Assert
                Assert.IsNull(orders);
            }

        }

        [TestMethod]
        public void UpdateOrderStatusAsync_UpdateOrderStatusToSent_ReturnTrue()
        {
            using (var context = new TestOrdersDbContext().DbContext)
            {
                // Arrange 
                var dummyOrder = DummyTestOrder.TestOrder();
                context.Order.Add(dummyOrder);
                context.SaveChanges();

                // Act
                var orderRepository = new OrderRepository(context);
                var result = orderRepository.UpdateOrderStatusAsync(2, dummyOrder.Id).Result;

                // Assert
                Assert.IsTrue(result);

                // Delete dummyOrder from DB
                context.Remove(dummyOrder);
                context.SaveChanges();
            }

        }

        [TestMethod]
        public void UpdateOrderStatusAsync_UpdateOrderStatusToNonExistingStatus_ReturnFalse()
        {
            using (var context = new TestOrdersDbContext().DbContext)
            {
                // Arrange 
                var dummyOrder = DummyTestOrder.TestOrder();
                context.Order.Add(dummyOrder);
                context.SaveChanges();

                // Act
                var orderRepository = new OrderRepository(context);
                var result = orderRepository.UpdateOrderStatusAsync(10, dummyOrder.Id).Result;

                // Assert
                Assert.IsFalse(result);

                // Delete dummyOrder from DB
                context.Remove(dummyOrder);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void UpdateOrderStatusAsync_TryUpdateStatusOfNonExistingOrder_ReturnFalse()
        {
            using (var context = new TestOrdersDbContext().DbContext)
            {
                // Arrange 
                var nonExistingOrderId = Guid.NewGuid();

                // Act
                var orderRepository = new OrderRepository(context);
                var result = orderRepository.UpdateOrderStatusAsync(3, nonExistingOrderId).Result;

                // Assert
                Assert.IsFalse(result);
            }
        }

    }
}
