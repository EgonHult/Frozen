using Frozen.Models;
using Frozen.Services;
using Frozen.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frozen.Controllers
{
    public class CartController : Controller
    {

        private readonly IClientService _clientService;
        public CartController(IClientService clientService)
        {
            this._clientService = clientService;
        }
        public IActionResult Index()
        {
            var cart = ShowCart();
            if (cart.CartItems.Count == 0)
            {
                ViewBag.Message = "Er kundvagn är tom";
                return View(cart);
            }
            else
                return View(cart);
        }

        public CartViewModel ShowCart()
        {
            var cart = GetCart();
            CartViewModel cartViewModel = new CartViewModel();

            if (cart != null)
            {

                cartViewModel.CartItems = cart;
                cartViewModel.TotalPrice = cart.Sum(cartItem => cartItem.Product.Price * cartItem.Quantity);
                
                return cartViewModel;
            }
            return cartViewModel;
        }

        public List<CartItem> GetCart()
        {
            return SessionHandler.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
        }
        public void SetCart(List<CartItem> cartItems)
        {
            SessionHandler.SetObjectAsJson(HttpContext.Session, "cart", cartItems); 
        }
        [HttpGet]
        public async Task<IActionResult> AddToCart(Guid productId)
        {
            var apiLocation = "https://localhost:44350/product/" + productId;
            var response = await _clientService.SendRequestToGatewayAsync(apiLocation, HttpMethod.Get);
            response.EnsureSuccessStatusCode();
            var product = await response.Content.ReadAsStringAsync();
            var cartItem = JsonConvert.DeserializeObject<Product>(product);
            List<CartItem> cart = GetCart();

            if (cart == null)
            {
                cart = new List<CartItem>()
                {
                    new CartItem{Product = cartItem, Quantity = 1}
                };
                SessionHandler.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                int index = FindIndexOfCartItem(cart, productId);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new CartItem { Product = cartItem, Quantity = 1 });
                }
                SessionHandler.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            var totalItemsInCart = cart.Sum(cartItem => cartItem.Quantity);
            return Ok(totalItemsInCart);
        }
        public IActionResult ReduceFromCart(Guid productId)
        {
            var cart = GetCart();
            if (cart != null)
            {
                int index = FindIndexOfCartItem(cart, productId);
                if (index != -1)
                {
                    cart[index].Quantity--;
                }
                SetCart(cart);
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult RemoveFromCart(Guid productId)
        {
            var cart = GetCart();
            if (cart != null)
            {
                int index = FindIndexOfCartItem(cart, productId);
                if (index != -1)
                {
                    cart.RemoveAt(index);
                }
                SetCart(cart);
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }
        public int FindIndexOfCartItem(List<CartItem> cartItems, Guid productId)
        {
            return cartItems.FindIndex(x => x.Product.Id == productId);
        }
    }
}
