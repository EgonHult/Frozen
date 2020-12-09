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
        public void CreateOrderAsync_TryCreateNullOrder_CatchExceptionReturnTrue()
        {
            using(var context = new TestOrdersDbContext().DbContext)
            {
                // Arrange
                var orderRepository = new OrderRepository(context);
                Order dummyOrder = null;

                try
                {
                    //Act 
                    var nullOrder = orderRepository.CreateOrderAsync(dummyOrder).Result;
                }
                catch (Exception)
                {

                    return;
                }
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
                var productRepository = new OrderRepository(context);
                var deletedProduct = productRepository.DeleteOrderByIdAsync(dummyOrder.Id);

                // Assert
                Assert.AreEqual(dummyOrder, deletedProduct.Result);

          
            }
        }

        [TestMethod]
        public void DeleteOrderByIdAsync_TryDeleteNonExistingOrder_ReturnNull()
        {
            using (var context = new TestOrdersDbContext().DbContext)
            {
                try
                {
                    // Arrange
                    var nonExistingOrderId = Guid.NewGuid();

                    // Act
                    var orderRepository = new OrderRepository(context);
                    var order = orderRepository.DeleteOrderByIdAsync(nonExistingOrderId);
                }
                catch (Exception)
                {
                    // Arrange
                    return;
                }
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
                var order = orderRepository.GetOrderByIdAsync(dummyOrder.Id).Result;

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
                var order = orderRepository.GetOrderByIdAsync(dummyOrder.Id).Result;

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
                Assert.IsInstanceOfType(orders, typeof(List<Order>));         
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



    }
}