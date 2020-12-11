using Frozen.Controllers;
using Frozen.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frozen.UnitTests
{
    [TestClass]
    public class ProductsTests
    {
        [TestMethod]
        public async void GetProducts_GetAllProducts_ReturnProductList()
        {
            //Arrange
            ProductsController controller = new ProductsController();
            //Act
            var products = await controller.GetProducts();
            //Assert
            Assert.AreSame(products, new List<Product>());
        }
    }
}
