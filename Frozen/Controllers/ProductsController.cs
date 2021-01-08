using Frozen.Common;
using Frozen.Models;
using Frozen.Services;
using Frozen.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ICookieHandler _cookieHandler;
        private readonly IClientService _clientService;

        public ProductsController(ICookieHandler cookieHandler, IClientService clientService)
        {
            this._cookieHandler = cookieHandler;
            this._clientService = clientService;
        }

        public async Task<IActionResult> ProductsPage()
        {
            var products = await GetProductsAsync();
            return View(products);
        }
        [HttpGet]
        public async Task<List<Product>> GetProductsAsync()
        {
            var apiLocation = "https://localhost:44350/product/getall";
            var response = await _clientService.SendRequestToGatewayAsync(apiLocation, HttpMethod.Get);
            response.EnsureSuccessStatusCode();
            var products = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Product>>(products);
            
        }
        [HttpGet]
        public async Task<Product> GetProductByIdAsync(Guid Id)
        {
            var apiLocation = "https://localhost:44350/product/"+ Id;
            var response = await _clientService.SendRequestToGatewayAsync(apiLocation, HttpMethod.Get);
            response.EnsureSuccessStatusCode();
            var product = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Product>(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<Product> PostProductAsync(Product product)
        {
            var jwtToken = _cookieHandler.ReadSessionCookieContent(Cookies.JWT_SESSION_TOKEN);

            var apiLocation = "https://localhost:44350/product/create";
            var response = await _clientService.SendRequestToGatewayAsync(apiLocation, HttpMethod.Post, product, jwtToken);
            response.EnsureSuccessStatusCode();
            var createdProduct = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Product>(createdProduct);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<Product> UpdateProductAsync(Guid Id, Product product)
        {
            var jwtToken = _cookieHandler.ReadSessionCookieContent(Cookies.JWT_SESSION_TOKEN);
            var apiLocation = "https://localhost:44350/product/" + Id;
            var response = await _clientService.SendRequestToGatewayAsync(apiLocation, HttpMethod.Put, product, jwtToken);
            response.EnsureSuccessStatusCode();
            var Updatedproduct = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Product>(Updatedproduct);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<Product> DeleteProductAsync(Guid Id)
        {
            var jwtToken = _cookieHandler.ReadSessionCookieContent(Cookies.JWT_SESSION_TOKEN);
            var apiLocation = "https://localhost:44350/product/" + Id;
            var response = await _clientService.SendRequestToGatewayAsync(apiLocation, HttpMethod.Delete, null, jwtToken);
            response.EnsureSuccessStatusCode();
            var product = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Product>(product);
        }
    }
}
