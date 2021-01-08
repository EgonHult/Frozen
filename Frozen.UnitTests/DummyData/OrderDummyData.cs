using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frozen.UnitTests.DummyData
{
    public class OrderDummyData
    {
        public static Order TestOrder()
        {

            var dummyOrder = new Order
            {
                Id = Guid.NewGuid(),
                StatusId = 1,
                TotalPrice = 1000.99M,
                PaymentId = 1,
                UserId = Guid.NewGuid(),
                Date = DateTime.Now,

                OrderProduct = new List<OrderProduct>()
                {
                    new OrderProduct { Quantity = 2, ProductId = Guid.NewGuid(), Name = "Isglass" },
                    new OrderProduct { Quantity = 2, ProductId = Guid.NewGuid(), Name = "Mjukglass" }
                }
            };

            return dummyOrder;

        }
    }
}
