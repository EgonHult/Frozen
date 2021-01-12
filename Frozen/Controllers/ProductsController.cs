using Frozen.Common;
using Frozen.Models;
using Frozen.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frozen.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IClientService _clientService;

        public ProductsController(IClientService clientService)
        {
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
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Products.ALL_PRODUCTS, HttpMethod.Get);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<List<Product>>(response.Content) : null;
        }

        [HttpGet]
        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Products.GATEWAY_BASEURL + id, HttpMethod.Get);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<Product>(response.Content) : null;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<Product> PostProductAsync(Product product)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Products.CREATE_PRODUCT, HttpMethod.Post, product);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<Product>(response.Content) : null;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<Product> UpdateProductAsync(Guid id, Product product)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Products.GATEWAY_BASEURL + id, HttpMethod.Put, product);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<Product>(response.Content) : null;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<Product> DeleteProductAsync(Guid id)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Products.GATEWAY_BASEURL + id, HttpMethod.Delete);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<Product>(response.Content) : null;
        }
    }
}
