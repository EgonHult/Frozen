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
                    Details = "Den godaste desserten.",
                    Quantity = 200,
                    WeightInGrams = 1000,
                    Image ="https://www.email-gourmand.com/media/zoo/images/sbm_glaces_0fe540aa7ca094d1d39d83c7f5a32e08.jpg"
                },

                new ProductModel
                {
                    Name = "Cheesecake",
                    Price = 50,
                    Details = "Fryst cheesecake är en perfekt dessert för stora kalas eller middagar. Ta fram cheesecaken en stund innan servering.",
                    Quantity = 100,
                    WeightInGrams = 500,
                    Image = "https://www.schweetfoods.com/uploads/6/9/5/2/6952941/mg-4964-edit_orig.jpg"
                },

                new ProductModel
                {
                    Name = "Fryst frukt",
                    Price = 40,
                    Details = "Fryst frukt är sådant en alltid bör ha hemma i frysen så att en snabbt kan tina upp då en vill ha bär i sin fil eller till glassen. Hallon och jordgubbar kan också ätas som de är med en klick grädde.",
                    Quantity = 100,
                    WeightInGrams = 1000,
                    Image = "https://media-manager.starsinsider.com/1080/na_5fa2d798ce428.jpg"
                },
                new ProductModel
                {
                    Name = "Minipaj",
                    Price = 50,
                    Details = "Läckra minipajer med mycket smak, som passar lika bra på sommarbuffén som till en enklare förrätt.",
                    Quantity = 75,
                    WeightInGrams = 750,
                    Image = "https://biancazapatka.com/wp-content/uploads/2019/10/apple-pie-mini-pies-muffins-vegan-dessert-recipe.jpg"

                },
                new ProductModel
                {
                    Name = "Lasagne",
                    Price = 50,
                    Details = "En klassisk lasagne brukar gå hem i alla hem och är en perfekt rätt. Den här varianten är så krämig och god!",
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
