using Microsoft.EntityFrameworkCore;
using Orders.Context;
using Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frozen.UnitTests.DummyData
{
    public class OrderDummyData
    {
        public OrderDbContext DbContext { get; set; }
        public OrderDummyData()
        {
            InitializeContext();
        }
        public void InitializeContext()
        {
            var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Frozen_Orders;Trusted_Connection=True;";
            var dbOption = new DbContextOptionsBuilder<OrderDbContext>()
                .UseSqlServer(connectionString).Options;

            DbContext = new OrderDbContext(dbOption);
        }
        public OrderModel CreateDummyOrder()
        {
            var dummyOrder = new OrderModel
            {
                StatusId = 1,
                TotalPrice = 1000.99M,
                PaymentId = 1,
                UserId = Guid.Parse("a5fa1093-aebc-49ac-8636-5c171c4d3991"),
                Date = DateTime.Now,

                OrderProduct = new List<OrderProductModel>()
                {
                    new OrderProductModel { Quantity = 2, ProductId = Guid.NewGuid(), Name = "Isglass" },
                    new OrderProductModel { Quantity = 2, ProductId = Guid.NewGuid(), Name = "Mjukglass" }
                }
            };
            DbContext.Order.Add(dummyOrder);
            DbContext.SaveChanges();
            return dummyOrder;
        }
        public void DeleteDummyOrder(Guid Id)
        {
            var order = DbContext.Order.Find(Id);
            DbContext.Remove(order);
            DbContext.SaveChanges();
        }
    }
}
