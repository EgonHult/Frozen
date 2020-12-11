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
            var products = await GetProductsAsync();
            return View(products);
        }
        [HttpGet]
        public async Task<List<Product>> GetProductsAsync()
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44350/product/getall");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var products = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Product>>(products);
            }
        }
        [HttpGet]
        public async Task<Product> GetProductByIdAsync(Guid Id)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44350/product/"+Id);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var product = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Product>(product);
            }
        }
        [HttpPost]
        public async Task<Product> PostProductAsync(Product product)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44350/product/");
                string json = JsonConvert.SerializeObject(product);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var createdProduct = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Product>(createdProduct);
            }
        }
        [HttpPut]
        public async Task<Product> UpdateProductAsync(Guid Id, Product product)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:44350/product/" + Id);
                string json = JsonConvert.SerializeObject(product);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var Updatedproduct = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Product>(Updatedproduct);
            }
        }
        [HttpDelete]
        public async Task<Product> DeleteProductAsync(Guid Id)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, "https://localhost:44350/product/" + Id);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var product = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Product>(product);
            }
        }
    }
}
