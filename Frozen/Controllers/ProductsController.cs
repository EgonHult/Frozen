using Frozen.Models;
using Frozen.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Frozen.Controllers
{
    public class ProductsController : Controller
    {
        public async Task<IActionResult> ProductsPage()
        {
            //var products = await GetProducts();
            var products = new List<Product>
            {
                new Product { Name = "Sak", Details = "En fin sak", Price = 100, Quantity = 2, Image = "https://cdn-rdb.arla.com/Files/arla-se/2788145226/7c41fbbb-9818-4b04-9fee-df737ace39b2.jpg?mode=crop&w=1200&h=630&scale=both&format=jpg&quality=80&ak=f525e733&hm=35af1404"},
                new Product { Name = "Grej", Details = "En cool grej", Price = 12312.54M, Quantity = 140, Image="https://lh3.googleusercontent.com/proxy/1CV_6fShzPE98_Oc0XiR2lTaFLlZ0x4ICeFMXBC27aukaZ5EYlywgtYz3HfzmEVA7ZxGH3maw0QLRG35NzgY7hEASW7g5w-mRIAHGjyyytNHmNbTO5xki1vD30HBugQsQ0yrrca5kBr8NF1vSrlYmApsh10xMg"},
                new Product { Name = "Mojäng", Details = "En fet mojäng", Price = 12324312.54M, Quantity = 14120, Image="https://images.recept.se/images/recipes/chokladtarta-med-paronfyllning-och-marsipan_7762.jpg?fit=crop&crop=focalpoint&fp-x=0.5&fp-y=0.37368514503589&fp-z=1&w=1200&h=628"},
                new Product { Name = "Pryl", Details = "En snygg pryl", Price = 12.5M, Quantity = 99, Image="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQhj9YrpDPFElDskSFYXKDfxiqG2G63alHCLw&usqp=CAU"},
                new Product { Name = "Objekt", Details = "Ett fränt objekt", Price = 1212.5M, Quantity = 1, Image="https://www.lecreuset.se/on/demandware.static/-/Library-Sites-lc-sharedLibrary/default/dwdedfde5e/images/recipe/HD_PNG_LC_20200430_DK_RC_CI_r0000000000543_DAN.png"}
            };
            return View(products);
        }
        [HttpGet]
        public async Task<List<Product>> GetProducts()
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44350/products");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var products = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Product>>(products);
            }
        }
    }
}
