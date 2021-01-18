using Frozen.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frozen.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor httpContext;

        public CartService(IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
        }
        public List<CartItem> GetCart()
        {
            return SessionHandler.GetObjectFromJson<List<CartItem>>(httpContext.HttpContext.Session, "cart");
        }
        public decimal CalculateTotalPrice()
        {
            var cart = GetCart();
            return cart.Sum(cartItem => cartItem.Product.Price * cartItem.Quantity);
        }
    }
}
