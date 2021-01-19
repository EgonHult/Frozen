using Microsoft.VisualStudio.TestTools.UnitTesting;
using Products.Context;
using Products.Models;
using Products.Repositories;
using Products.UnitTest.Context;
using Products.UnitTest.DummyProduct;
using System;
using System.Collections.Generic;

namespace Products.UnitTest
{
    [TestClass]
    public class ProductsTest
    {

        public static IProductRepository ProductRepository { get; set; }
        public static TestProductsDbContext ProductTestContext { get; set; }

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            ProductTestContext = new TestProductsDbContext();
            ProductRepository = new ProductRepository(ProductTestContext.DbContext);
        }


        [TestMethod]
        public void CreateProductAsync_CreateNewProduct_ReturnCreatedProduct()
        {
           
            // Arrange              
            var dummyProduct = DummyTestProduct.TestProduct();

            // Act
            var newProduct = ProductRepository.CreateProductAsync(dummyProduct).Result;

            // Assert
            Assert.AreEqual(dummyProduct, newProduct);

            // Clean up!
            ProductTestContext.DbContext.Remove(newProduct);
            ProductTestContext.DbContext.SaveChanges();
            
        }

        [TestMethod]
        public void CreateProductAsync_TryCreateNullProduct_ReturnNull()
        {         
            // Arrange            
            ProductModel dummyProduct = null;
           
            // Act
            var nullProduct = ProductRepository.CreateProductAsync(dummyProduct).Result;

            // Assert
            Assert.IsNull(nullProduct);                    
        }

        [TestMethod]
        public void CreateOrderAsync_TryCreateProductWithExistingProductId_ReturnNull()
        {

            // Arrange               
            var dummyProduct = DummyTestProduct.TestProduct();
            var dummyProduct2 = dummyProduct;

            //Act 

            var newOrder = ProductRepository.CreateProductAsync(dummyProduct).Result;
            var newOrder2 = ProductRepository.CreateProductAsync(dummyProduct2).Result;

            //Assert
            Assert.AreEqual(newOrder2, null);

            // Delete dummyOrder from DB
            ProductTestContext.DbContext.Remove(newOrder);
            ProductTestContext.DbContext.SaveChanges();
        }

        [TestMethod]
        public void DeleteProductByIdAsync_DeleteProductFromDatabase_ReturnDeletedProductAreEqual()
        {         
            // Arrange
            var dummyProduct = DummyTestProduct.TestProduct();
            ProductTestContext.DbContext.Product.Add(dummyProduct);
            ProductTestContext.DbContext.SaveChanges();

            // Act
            var deletedProduct = ProductRepository.DeleteProductByIdAsync(dummyProduct.Id).Result;

            // Assert
            Assert.AreEqual(dummyProduct, deletedProduct);            
        }

        [TestMethod]
        public void DeleteProductByIdAsync_TryDeleteNonExistingProduct_ReturnNull()
        {          
            try
            {
                // Arrange
                var nonExistingProductId = Guid.NewGuid();

                // Act                  
                var product = ProductRepository.DeleteProductByIdAsync(nonExistingProductId).Result;
            }
            catch (Exception)
            {
                // Arrange
                return;
            }
            
        }

        [TestMethod]
        public void DeleteProductByIdAsync_TryDeleteProductByEmptyId_ReturnNull()
        {
            //Arrange
            var emptyProductId = Guid.Empty;

            //Act              
            var result = ProductRepository.DeleteProductByIdAsync(emptyProductId).Result;

            //Assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public void GetAllProducts_GetListWithAllProductsInDb_ReturnListOfProducts()
        {
            // Act             
            var products = ProductRepository.GetAllProductsAsync().Result;

            // Assert
            Assert.IsInstanceOfType(products, typeof(List<ProductModel>));           
        }

        [TestMethod]
        public void GetProductByIdAsync_GetProductById_ReturnProduct()
        {
           
            // Arrange 
            var dummyProduct = DummyTestProduct.TestProduct();
            ProductTestContext.DbContext.Product.Add(dummyProduct);
            ProductTestContext.DbContext.SaveChanges();

            // Act
            var product = ProductRepository.GetProductByIdAsync(dummyProduct.Id).Result;

            // Assert
            Assert.IsNotNull(product);

            // Delete dummyOrder from DB
            ProductTestContext.DbContext.Remove(product);
            ProductTestContext.DbContext.SaveChanges();
            
        }

        [TestMethod]
        public void UpdateProductAsync_UpdatePriceForProduct_ReturnUpdatedProduct()
        {
           
            // Arrange
            var dummyProduct = DummyTestProduct.TestProduct();
            ProductTestContext.DbContext.Product.Add(dummyProduct);
            ProductTestContext.DbContext.SaveChanges();
            var oldPrice = 1000;

            // Act
            dummyProduct.Price = 2000;
            var product = ProductRepository.UpdateProductAsync(dummyProduct).Result;

            // Assert
            Assert.AreNotEqual(oldPrice, product.Price);
            Assert.AreEqual(dummyProduct.Price, product.Price);

            // Delete dummyOrder from DB
            ProductTestContext.DbContext.Remove(dummyProduct);
            ProductTestContext.DbContext.SaveChanges();
          
        }

        [TestMethod]
        public void UpdateProductAsync_TryUpdateNonExistingProduct_ReturnNull()
        {          
            try
            {
                // Arrange
                ProductModel dummyProduct = null;

                // Act            
                var product = ProductRepository.UpdateProductAsync(dummyProduct).Result;
            }
            catch (Exception)
            {
                // Arrange
                return;
            }
            
        }

    }
}
