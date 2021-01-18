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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageProductsPage()
        {
            var products = await GetProductsAsync();
            return View(products);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProductPage(Guid Id)
        {
            var product = await GetProductByIdAsync(Id);
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditProductPageAsync(Product product)
        {
            if (ModelState.IsValid)
            {
                var responseProduct = await UpdateProductAsync(product.Id, product);
                if (responseProduct != null)
                {
                    TempData["AlertMessage"] = "Produkten är uppdaterad!";
                    return RedirectToAction("ManageProductsPage");
                }
            }
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid Id)
        {
            var response = await DeleteProductAsync(Id);
            if (response != null)
            {
                TempData["AlertMessage"] = "Produkten borttagen!";
                return RedirectToAction("ManageProductsPage");
            }
            TempData["AlertMessage"] = "Kunde inte ta bort produkten!";
            return RedirectToAction("ManageProductsPage");
        }

        public async Task<IActionResult> ProductDetails(Guid id)
        {
            var product = await GetProductByIdAsync(id);
            return View(product);
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
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Products.EDIT_PRODUCT + id, HttpMethod.Put, product);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<Product>(response.Content) : null;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<Product> DeleteProductAsync(Guid id)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Products.DELETE_PRODUCT + id, HttpMethod.Delete);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<Product>(response.Content) : null;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateProductPage()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProductPage(Product product)
        {
            if (ModelState.IsValid)
            {
                var result = await PostProductAsync(product);
                if(result != null)
                {
                    TempData["AlertMessage"] = "Ny produkt tillagd!";
                    return RedirectToAction("ManageProductsPage");
                }
            }
            return View();
        }
    }
}
