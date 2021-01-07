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
        public void GetProducts_GetAllProducts_ReturnProductList()
        {
            //Arrange
            ProductsController controller = new ProductsController();
            //Act
            var products = controller.GetProductsAsync().Result;
            //Assert
            Assert.IsInstanceOfType(products, typeof(List<Product>));
            Assert.IsTrue(products.Count > 0);
        }
        [TestMethod]
        public void GetProductById_GetProduct_ReturnProduct()
        {
            //Arrange
            ProductsController controller = new ProductsController();
            var id = new Guid();
            //Act
            var product = controller.GetProductByIdAsync(id).Result;
            //Assert
            Assert.AreEqual(product.Id, id);
        }
        [TestMethod]
        public void PostProduct_PostProduct_ReturnPostedProduct()
        {
            //Arrange
            Product product = new Product
            {
                Name = "Test",
                Details = "Test product",
                Price = 100,
                Quantity = 50,
                WeightInGrams = 100,
                Image = ""
            };
            ProductsController controller = new ProductsController();
            //Act
            var result = controller.PostProductAsync(product).Result;
            //Assert
            Assert.AreEqual(result, product);
        }
        [TestMethod]
        public void UpdateProduct_UpdateProduct_ReturnUpdatedProduct()
        {
            //Arrange
            Guid guid = new Guid();
            Product product = new Product
            {
                Id = guid,
                Name = "Uppdaterat namn",
                Details = "Uppdaterade detaljer",
                Image = "",
                WeightInGrams = 100,
                Price = 2200,
                Quantity = 5
            };
            ProductsController controller = new ProductsController();
            //Act
            var result = controller.UpdateProductAsync(guid, product).Result;
            //Assert
            Assert.AreEqual(guid, result.Id);
            Assert.AreEqual(result.Name, product.Name);
        }
        [TestMethod]
        public void DeleteProduct_DeleteProduct_ReturnDeletedProduct()
        {
            //Arrange
            var Id = new Guid();
            ProductsController controller = new ProductsController();

            //Act
            var result = controller.DeleteProductAsync(Id).Result;
            //Assert
            Assert.AreEqual(result.Id, Id);
        }
    }
}
