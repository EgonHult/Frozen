using Frozen.Controllers;
using Frozen.Models;
using Frozen.UnitTests.DummyData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Products.Models;
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
        static ProductDummyData DummyData { get; set; }
        static ProductModel DummyProduct { get; set; }
        [ClassInitialize]
        public static void TestFixtureSetup(TestContext Context)
        {
            DummyData = new ProductDummyData();
            DummyProduct = DummyData.CreateDummyProduct();
        }
        [ClassCleanup]
        public static void TestFixtureDispose()
        {
            DummyData.DeleteDummyProduct(DummyProduct.Id);
        }
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
            var id = DummyProduct.Id;
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
                Name = "Test2",
                Details = "Test product2",
                Price = 100,
                Quantity = 50,
                WeightInGrams = 100,
                Image = ""
            };
            ProductsController controller = new ProductsController();
            //Act
            var result = controller.PostProductAsync(product).Result;
            //Assert
            Assert.AreEqual(result.Name, product.Name);
            DummyData.DeleteDummyProduct(result.Id);
        }
        [TestMethod]
        public void UpdateProduct_UpdateProduct_ReturnUpdatedProduct()
        {
            //Arrange
            Product product = new Product
            {
                Id = DummyProduct.Id,
                Name = "Uppdaterat namn",
                Details = "Uppdaterade detaljer",
                Image = "",
                WeightInGrams = 100,
                Price = 2200,
                Quantity = 5
            };
            ProductsController controller = new ProductsController();
            //Act
            var result = controller.UpdateProductAsync(DummyProduct.Id, product).Result;
            //Assert
            Assert.AreEqual(DummyProduct.Id, result.Id);
            Assert.AreEqual(result.Name, product.Name);
        }
        [TestMethod]
        public void DeleteProduct_DeleteProduct_ReturnDeletedProduct()
        {
            //Arrange
            var DummyProductTwo = DummyData.CreateDummyProduct();
            var Id = DummyProductTwo.Id;
            ProductsController controller = new ProductsController();
            //Act
            var result = controller.DeleteProductAsync(Id).Result;
            //Assert
            Assert.AreEqual(result.Id, Id);
            if(result == null)
            {
                DummyData.DeleteDummyProduct(Id);
            }
        }
        
    }
}
