using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Orders.Models;
using Orders.Repositories;
using Orders.UnitTest.Context;
using Orders.UnitTest.DummyOrder;



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
                var newOrder = orderRepository.CreateOrderAsync(dummyOrder);

                // Assert
                Assert.AreEqual(dummyOrder, newOrder.Result);

                //context.Remove(dummyOrder);
                //context.SaveChanges();


            }
        }


   }
}
