using Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Context
{
    public static class DbInitializer
    {
        public static void Initialize(ProductsDbContext context)
        {
            context.Database.EnsureCreated();
            var result = context.Product.ToList();
            if (result.Count > 0)
            {
                return;
            }

            var products = new ProductModel[]
            {
                new ProductModel
                {
                    Name = "Glass",
                    Price = 30,
                    Details = "Smarrigt",
                    Quantity = 200,
                    WeightInGrams = 1000,
                    Image ="https://www.email-gourmand.com/media/zoo/images/sbm_glaces_0fe540aa7ca094d1d39d83c7f5a32e08.jpg"
                },

                new ProductModel
                {
                    Name = "Cheesecake",
                    Price = 50,
                    Quantity = 100,
                    WeightInGrams = 500,
                    Image = "https://www.schweetfoods.com/uploads/6/9/5/2/6952941/mg-4964-edit_orig.jpg"
                },

                new ProductModel
                {
                    Name = "Fryst frukt",
                    Price = 40,
                    Quantity = 100,
                    WeightInGrams = 1000,
                    Image = "https://media-manager.starsinsider.com/1080/na_5fa2d798ce428.jpg"
                },
                new ProductModel
                {
                    Name = "Minipajer",
                    Price = 50,
                    Quantity = 75,
                    WeightInGrams = 750,
                    Image = "https://biancazapatka.com/wp-content/uploads/2019/10/apple-pie-mini-pies-muffins-vegan-dessert-recipe.jpg"

                },
                new ProductModel
                {
                    Name = "Lasagne",
                    Price = 50,
                    Quantity = 50,
                    WeightInGrams = 800,
                    Image = "https://www.cookingclassy.com/wp-content/uploads/2020/03/lasagna-18.jpg"
                }

            };

            foreach(ProductModel p in products)
            {
                context.Product.Add(p);
            }
            context.SaveChanges();
        }
    }
}
