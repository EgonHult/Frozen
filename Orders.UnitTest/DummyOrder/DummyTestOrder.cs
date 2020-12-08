using Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.UnitTest.DummyOrder
{
    public class DummyTestOrder
    {
        public static Order TestOrder()
        {
            var dummyStatus = new Status
            {
                Id = Guid.NewGuid(),
                Name = "On Hold"
            };

            
            var dummyOrder = new Order
            {
                Id = Guid.NewGuid(),
                StatusId = dummyStatus.Id,
                TotalPrice = 1000,               
                PaymentId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),

                OrderProduct = new List<OrderProduct>()
                {
                    new OrderProduct { Amount = 2, ProductId = Guid.NewGuid() },
                    new OrderProduct { Amount = 2, ProductId = Guid.NewGuid() }
                }
                
            };
        

            return dummyOrder;

        }
    }
}
