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
        public async void GetProductsAsync_GetAllProducts_ReturnProductList()
        {
            //Arrange
            ProductsController controller = new ProductsController();
            //Act
            var products = await controller.GetProductsAsync();
            //Assert
            Assert.AreSame(products, new List<Product>());
            Assert.AreEqual(products.Count, 5);
        }
        [TestMethod]
        public async void GetProductByIdAsync_GetProduct_ReturnProduct()
        {
            //Arrange
            ProductsController controller = new ProductsController();
            var id = new Guid();
            //Act
            var product = await controller.GetProductByIdAsync(id);
            //Assert
            Assert.AreEqual(product.Id, id);
        }
        [TestMethod]
        public async void PostProductAsync_PostProduct_ReturnPostedProduct()
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
            //var result =  await controller.PostProductAsync(product);
            //Assert
            //Assert.AreEqual(result, product);
        }
        [TestMethod]
        public async void UpdateProductAsync_UpdateProduct_ReturnUpdatedProduct()
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
            var result = await controller.UpdateProductAsync(guid, product);
            //Assert
            Assert.AreEqual(guid, result.Id);
            Assert.AreEqual(result.Name, product.Name);
        }
        [TestMethod]
        public async void DeleteProductAsync_DeleteProduct_ReturnDeletedProduct()
        {
            //Arrange
            var Id = new Guid();
            ProductsController controller = new ProductsController();

            //Act
            var result = await controller.DeleteProductAsync(Id);
            //Assert
            Assert.AreEqual(result.Id, Id);
        }
    }
}
