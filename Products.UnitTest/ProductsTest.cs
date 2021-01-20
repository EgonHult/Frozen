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
            DeleteDummyProductFromDatabase(dummyProduct);
        }
      

        [TestMethod]
        public void CreateProductAsync_TryCreateNullProduct_ReturnNull()
        {         
            // Arrange            
            ProductModel dummyProduct = null;
           
            // Act
            var product = ProductRepository.CreateProductAsync(dummyProduct).Result;

            // Assert
            Assert.IsNull(product);                    
        }

        [TestMethod]
        public void CreateProductAsync_TryCreateProductWithEmptyModel_ReturnNull()
        {
            //Arrange
            var ProductModel = new ProductModel();

            //Act
            var product = ProductRepository.CreateProductAsync(ProductModel).Result;

            //Assert
            Assert.IsNull(product);
        }

        [TestMethod]
        public void CreateProductAsync_TryCreateProductWithAllReadyExistingProductIdInDatabase_ReturnNull()
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
            DeleteDummyProductFromDatabase(dummyProduct);
        }

        [TestMethod]
        public void DeleteProductByIdAsync_DeleteProductFromDatabase_ReturnDeletedProductAreEqual()
        {
            // Arrange
            ProductModel dummyProduct = CreateDummyProductToDatabase();

            // Act
            var deletedProduct = ProductRepository.DeleteProductByIdAsync(dummyProduct.Id).Result;

            // Assert
            Assert.AreEqual(dummyProduct, deletedProduct);            
        }

        [TestMethod]
        public void DeleteProductByIdAsync_TryDeleteNonExistingProduct_ReturnNull()
        {                     
            // Arrange
            var nonExistingProductId = Guid.NewGuid();

            // Act                  
            var product = ProductRepository.DeleteProductByIdAsync(nonExistingProductId).Result;

            // Assert
            Assert.IsNull(product);  
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
            ProductModel dummyProduct = CreateDummyProductToDatabase();

            // Act
            var product = ProductRepository.GetProductByIdAsync(dummyProduct.Id).Result;

            // Assert
            Assert.IsNotNull(product);

            // Delete dummyOrder from DB
            DeleteDummyProductFromDatabase(dummyProduct);
        }

      

        [TestMethod]
        public void GetProductByIdAsync_TryGetNonExistingProduct_ReturnNull()
        {
            // Arrange 
            var dummyProduct = DummyTestProduct.TestProduct();

            // Act              
            var product= ProductRepository.GetProductByIdAsync(dummyProduct.Id).Result;

            // Assert
            Assert.IsNull(product);
        }

        [TestMethod]
        public void UpdateProductAsync_UpdatePriceForProduct_ReturnUpdatedProduct()
        {
            // Arrange
            ProductModel dummyProduct = CreateDummyProductToDatabase();
            var oldPrice = 1000;

            // Act
            dummyProduct.Price = 2000;
            var product = ProductRepository.UpdateProductAsync(dummyProduct).Result;

            // Assert
            Assert.AreNotEqual(oldPrice, product.Price);
            Assert.AreEqual(dummyProduct.Price, product.Price);

            // Delete dummyOrder from DB
            DeleteDummyProductFromDatabase(dummyProduct);
        }

        [TestMethod]
        public void UpdateProductAsync_TryUpdateNullProduct_ReturnNull()
        {
            // Arrange
            ProductModel dummyProduct = null;

            // Act            
            var product = ProductRepository.UpdateProductAsync(dummyProduct).Result;

            // Assert
            Assert.IsNull(product);                       
        }

        [TestMethod]
        public void UpdateProductAsync_TryUpdateNotExistingProduct_ReturnNull()
        {
            // Arrange
            var dummyProduct = DummyTestProduct.TestProduct();

            // Act            
            var product = ProductRepository.UpdateProductAsync(dummyProduct).Result;

            // Assert
            Assert.IsNull(product);
        }

        [TestMethod]
        public void UpdateProductsInStockAsync_UpdateProductQuantityInStock_ReturnTrue()
        {
            ProductModel dummyProduct = CreateDummyProductToDatabase();

            var quantityToDecreaseFromStock = new Dictionary<Guid, int>
            {
                { dummyProduct.Id, 50 }
            };
            var response = ProductRepository.UpdateProductsInStockAsync(quantityToDecreaseFromStock).Result;

            Assert.IsTrue(response);

            DeleteDummyProductFromDatabase(dummyProduct);
        }

        [TestMethod]
        public void UpdateProductsInStockAsync_TryUpdateProductQuantityInStockWithNoExistingProduct_ReturnFalse()
        {
            var dummyProduct = DummyTestProduct.TestProduct();

            var quantityToDecreaseFromStock = new Dictionary<Guid, int>
            {
                { dummyProduct.Id, 50 }
            };
            var response = ProductRepository.UpdateProductsInStockAsync(quantityToDecreaseFromStock).Result;

            Assert.IsFalse(response);
        }

        private static ProductModel CreateDummyProductToDatabase()
        {
            var dummyProduct = DummyTestProduct.TestProduct();
            ProductTestContext.DbContext.Product.Add(dummyProduct);
            ProductTestContext.DbContext.SaveChanges();

            return dummyProduct;
        }

        private static void DeleteDummyProductFromDatabase(ProductModel newProduct)
        {
            ProductTestContext.DbContext.Remove(newProduct);
            ProductTestContext.DbContext.SaveChanges();
        }


    }
}


