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
        private readonly IHttpContextAccessor _context;

        public CartService(IHttpContextAccessor httpContext)
        {
            this._context = httpContext;
        }

        public List<CartItem> GetCart()
        {
            return SessionHandler.GetObjectFromJson<List<CartItem>>(_context.HttpContext.Session, "cart");
        }

        public decimal CalculateTotalPrice()
        {
            var cart = GetCart();

            if (cart == null)
                return 0;

            return cart.Sum(cartItem => cartItem.Product.Price * cartItem.Quantity);
        }

        public void EmptyCart()
        {
            _context.HttpContext.Session.Remove("cart");
        }
    }
}
