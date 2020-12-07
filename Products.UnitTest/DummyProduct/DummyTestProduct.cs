using Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.UnitTest.DummyProduct
{
    public class DummyTestProduct
    {
        public static ProductModel TestProduct()
        {
            var dummyProduct = new ProductModel()
            {
                Name = "TestProduct",
                Price = 1000,
                Details = "Test details",
                Quantity = 200,
                Image = "testimage.jpg"
            };

            return dummyProduct;
        }
    }
}
