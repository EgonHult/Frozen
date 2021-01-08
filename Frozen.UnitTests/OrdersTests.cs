using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        [TestMethod]
        public void GetOrders_GetAllOrders_ReturnAllOrders()
        {
            //Act
            var orders = Controller.GetOrdersAsync().Result;
        }
    }
}
