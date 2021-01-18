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
        /*
        public static TestProductsDbContext DbContext { get; set; }
        
        [ClassInitialize]
        public static void LoadAppsettings(TestContext context)
        {
            DbContext = new TestProductsDbContext();
        }
        */

        [TestMethod]
        public void CreateProductAsync_CreateNewPRoduct_ReturnCreatedProduct()
        {
            using(var context = new TestProductsDbContext().DbContext)
            {
                // Arrange
                var productRepository = new ProductRepository(context);

                var dummyProduct = DummyTestProduct.TestProduct();

                // Act
                var newProduct = productRepository.CreateProductAsync(dummyProduct);

                // Assert
                Assert.AreEqual(dummyProduct, newProduct.Result);

                // Clean up!
                context.Remove(dummyProduct);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void CreateProductAsync_TryCreateNullProduct_CatchExceptionReturnTrue()
        {
            using (var context = new TestProductsDbContext().DbContext)
            {
                // Arrange
                var productRepository = new ProductRepository(context);

                ProductModel dummyProduct = null;

                try
                {
                    // Act
                    var nullProduct = productRepository.CreateProductAsync(dummyProduct);
                }
                catch(Exception)
                {
                    // Assert
                    return;
                }

            }
        }

        [TestMethod]
        public void DeleteProductByIdAsync_DeleteProductFromDatabse_ReturnDeletedProductAreEqual()
        {
            using (var context = new TestProductsDbContext().DbContext)
            {
                // Arrange
                var dummyProduct = DummyTestProduct.TestProduct();
                context.Product.Add(dummyProduct);
                context.SaveChanges();

                // Act
                var productRepository = new ProductRepository(context);
                var deletedProduct = productRepository.DeleteProductByIdAsync(dummyProduct.Id);

                // Assert
                Assert.AreEqual(dummyProduct, deletedProduct.Result);
            }
        }

        [TestMethod]
        public void DeleteProductByIdAsync_TryDeleteNonExistingProduct_ReturnNull()
        {
            using (var context = new TestProductsDbContext().DbContext)
            {
                try
                {
                    // Arrange
                    var nonExistingProductId = Guid.NewGuid();

                    // Act
                    var productRepository = new ProductRepository(context);
                    var product = productRepository.DeleteProductByIdAsync(nonExistingProductId);
                }
                catch (Exception)
                {
                    // Arrange
                    return;
                }
            }
        }
        [TestMethod]
        public void GetAllProducts_GetListWithAllProductsInDb_ReturnListOfProducts()
        {
            using (var context = new TestProductsDbContext().DbContext)
            {
                // Act
                var productRepository = new ProductRepository(context);
                var products = productRepository.GetAllProductsAsync().Result;

                // Assert
                Assert.IsInstanceOfType(products, typeof(List<ProductModel>));
            }
        }

        [TestMethod]
        public void GetProductByIdAsync_GetProductById_ReturnProduct()
        {
            using (var context = new TestProductsDbContext().DbContext)
            {
                // Arrange 
                var dummyProduct = DummyTestProduct.TestProduct();
                context.Product.Add(dummyProduct);
                context.SaveChanges();

                // Act
                var productRepository = new ProductRepository(context);
                var product = productRepository.GetProductByIdAsync(dummyProduct.Id).Result;

                // Assert
                Assert.IsNotNull(product);

                // Delete dummyOrder from DB
                context.Remove(dummyProduct);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void UpdateProductAsync_UpdatePriceForProduct_ReturnUpdatedProduct()
        {
            using (var context = new TestProductsDbContext().DbContext)
            {
                // Arrange
                var dummyProduct = DummyTestProduct.TestProduct();
                context.Product.Add(dummyProduct);
                context.SaveChanges();
                var oldPrice = 1000;

                // Act
                dummyProduct.Price = 2000;
                var productRepository = new ProductRepository(context);
                var product = productRepository.UpdateProductAsync(dummyProduct).Result;

                // Assert
                Assert.AreNotEqual(oldPrice, product.Price);
                Assert.AreEqual(dummyProduct.Price, product.Price);

                // Delete dummyOrder from DB
                context.Remove(dummyProduct);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void UpdateProductAsync_TryUpdateNonExistingProduct_ReturnNull()
        {
            using (var context = new TestProductsDbContext().DbContext)
            {
                try
                {
                    // Arrange
                    ProductModel dummyProduct = null;

                    // Act
                    var productRepository = new ProductRepository(context);
                    var product = productRepository.UpdateProductAsync(dummyProduct);
                }
                catch (Exception)
                {
                    // Arrange
                    return;
                }
            }
        }

    }
}
