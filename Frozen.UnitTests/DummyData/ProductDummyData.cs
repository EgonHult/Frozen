using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Products.Context;
using Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frozen.UnitTests.DummyData
{
    public class ProductDummyData
    {
        public ProductsDbContext DbContext { get; set; }
        public ProductDummyData()
        {
            InitializeContext();
        }
        public void InitializeContext()
        {
            var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Frozen_Products;Trusted_Connection=True;";
            var dbOption = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseSqlServer(connectionString).Options;

            DbContext = new ProductsDbContext(dbOption);
        }
        public ProductModel CreateDummyProduct()
        {
            ProductModel product = new ProductModel()
            {
                Name = "Test",
                Details = "Test product",
                Price = 100,
                Quantity = 50,
                WeightInGrams = 100,
                Image = ""
            };
            DbContext.Product.Add(product);
            DbContext.SaveChanges();

            return product;
        }
        public void DeleteDummyProduct(Guid id)
        {
            var product = DbContext.Product.Find(id);
            DbContext.Remove(product);
            DbContext.SaveChanges();
        }
    }
}
