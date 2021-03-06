﻿using Frozen.Common;
using Frozen.Controllers;
using Frozen.Models;
using Frozen.Services;
using Frozen.UnitTests.DummyData;
using Frozen.UnitTests.Sessions;
using Microsoft.AspNetCore.Http;
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
        static string JwtToken { get; set; }

        static ProductsController Controller { get; set; }

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext Context)
        {
            DummyData = new ProductDummyData();
            DummyProduct = DummyData.CreateDummyProduct();

            // Initialize HttpContext for accessing session functionality
            var config = new HttpContextConfig();
            IHttpContextAccessor httpContext = config.HttpContext;
            ICookieHandler _cookieHandler = new CookieHandler(httpContext);
            IClientService clientService = new ClientService(_cookieHandler);

            // Mock a session cookie with JwtToken
            JwtToken = AdminToken.LoginAdmin(clientService);
            _cookieHandler.CreateSessionCookie(Cookies.JWT_SESSION_TOKEN, JwtToken);

            // Arrange for all tests
            Controller = new ProductsController(clientService);

        }

        [ClassCleanup]
        public static void TestFixtureDispose()
        {
            DummyData.DeleteDummyProduct(DummyProduct.Id);
        }

        [TestMethod]
        public void GetProductsAsync_GetAllProducts_ReturnAllProducts()
        {
            //Act
            var products = Controller.GetProductsAsync().Result;

            //Assert
            Assert.IsInstanceOfType(products, typeof(List<Product>));
            Assert.IsTrue(products.Count > 0);
        }

        [TestMethod]
        public void GetProductByIdAsync_GetProduct_ReturnProduct()
        {
            //Arrange
            var id = DummyProduct.Id;

            //Act
            var product = Controller.GetProductByIdAsync(id).Result;

            //Assert
            Assert.AreEqual(product.Id, id);
        }

        [TestMethod]
        public void GetProductByIdAsync_TryGetProductWithNonExistingId_ReturnNull()
        {
            var id = Guid.NewGuid();

            var result = Controller.GetProductByIdAsync(id).Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void PostProductAsync_PostProduct_ReturnPostedProduct()
        {
            //Arrange
            Product product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test2",
                Details = "Test product2",
                Price = 100,
                Quantity = 50,
                WeightInGrams = 100,
                Image = "",
            };

            //Act
            var result = Controller.PostProductAsync(product).Result;

            //Assert
            Assert.AreEqual(result.Name, product.Name);
            DummyData.DeleteDummyProduct(result.Id);
        }

        [TestMethod]
        public void PostProductAsync_TryCreateNewProductWithEmptyProductModel_ReturnNull()
        {
           Product dummyProduct = new Product();

            var result = Controller.PostProductAsync(dummyProduct).Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void PostOrderAsync_TryCreateNewOrderWithNullOrder_ReturnNull()
        {
            Product dummyProduct = null;

            var result = Controller.PostProductAsync(dummyProduct).Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void UpdateProductAsync_UpdateProduct_ReturnUpdatedProduct()
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

            //Act
            var result = Controller.UpdateProductAsync(DummyProduct.Id, product).Result;

            //Assert
            Assert.AreEqual(DummyProduct.Id, result.Id);
            Assert.AreEqual(result.Name, product.Name);
        }

        [TestMethod]
        public void UpdateProductAsync_UpdateProductWithNotExistingProduct_ReturnNull()
        {
            //Arrange
            Product product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Uppdaterat namn",
                Details = "Uppdaterade detaljer",
                Image = "",
                WeightInGrams = 100,
                Price = 2200,
                Quantity = 5
            };

            //Act
            var result = Controller.UpdateProductAsync(DummyProduct.Id, product).Result;

            //Assert
            Assert.IsNull(result);            
        }

        [TestMethod]
        public void DeleteProductAsync_DeleteProduct_ReturnDeletedProduct()
        {
            //Arrange
            var DummyProductTwo = DummyData.CreateDummyProduct();
            var Id = DummyProductTwo.Id;

            //Act
            var result = Controller.DeleteProductAsync(Id).Result;

            //Assert
            Assert.AreEqual(result.Id, Id);
            if(result == null)
            {
                DummyData.DeleteDummyProduct(Id);
            }
        }

        [TestMethod]
        public void DeleteProductAsync_DeleteProductWithNotExistingProductId_ReturnNull()
        {
            var productId = Guid.NewGuid();

            var result = Controller.DeleteProductAsync(productId).Result;

            Assert.IsNull(result);
        }

    }
}
