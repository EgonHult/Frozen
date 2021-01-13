using Frozen.Common;
using Frozen.Models;
using Frozen.Services;
using Frozen.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
                cartViewModel.TotalPrice = CalculateTotalPrice();

                return cartViewModel;
            }
            return cartViewModel;
        }

        public decimal CalculateTotalPrice()
        {
            var cart = GetCart();
            return cart.Sum(cartItem => cartItem.Product.Price * cartItem.Quantity);        
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
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Products.GATEWAY_BASEURL + productId, HttpMethod.Get);
            var cartItem = await _clientService.ReadResponseAsync<Product>(response.Content);

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
            int totalItemsInCart = CountProductsInCart();
            return Ok(totalItemsInCart);
        }

        public int CountProductsInCart()
        {
            var cart = GetCart();
            if (cart == null)
            {
                return 0;
            }

            return cart.Sum(cartItem => cartItem.Quantity);
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
                    SetCart(cart);
                }
                if (cart[index].Quantity < 1)
                {
                    RemoveFromCart(productId);
                }

                return Ok(CountProductsInCart());
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

                return Ok(CountProductsInCart());
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
