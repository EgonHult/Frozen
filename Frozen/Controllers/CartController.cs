using Frozen.Common;
using Frozen.Models;
using Frozen.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frozen.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly IClientService _clientService;

        public CartController(IClientService clientService, CartService cartService)
        {
            this._clientService = clientService;
            this._cartService = cartService;
        }

        public IActionResult Index()
        {
            var cart = _cartService.ShowCart();

            if (cart.CartItems.Count == 0)
            {
                ViewBag.Message = "Er kundvagn är tom";
            }

            return View(cart);
        }

        public ActionResult CalculateTotalPrice()
        {
            return Ok(_cartService.CalculateTotalPrice());
        }

        public async Task<IActionResult> AddToCart(Guid productId)
        {
            if (!_cartService.ProductExistInCart(productId))
            {
                var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Products.GATEWAY_BASEURL + productId, HttpMethod.Get);
                var product = await _clientService.ReadResponseAsync<Product>(response.Content);
                return Ok(_cartService.AddNewProductToCart(product));
            }

            return Ok(_cartService.IncreaseExistingProductQuantity(productId));
        }

        public int CountProductsInCart()
        {
            return _cartService.CountProductsInCart();
        }

        public IActionResult ReduceFromCart(Guid productId)
        {
            return Ok(_cartService.ReduceExistingProductQuantity(productId));
        }

        public ActionResult RemoveFromCart(Guid productId)
        {
            var productsInCart = _cartService.RemoveProductFromCart(productId);
            return Ok(productsInCart);
        }

    }
}
