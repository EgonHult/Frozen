﻿using Frozen.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frozen.Services
{
    public class OrderService
    {
        private readonly ICookieHandler _cookieHandler;
        private readonly CartService _cartService;

        public OrderService(ICookieHandler cookieHandler, CartService cartService)
        {
            this._cookieHandler = cookieHandler;
            this._cartService = cartService;
        }

        /// <summary>
        /// Build new Order-object from products stored in cart session cookie
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>Order</returns>
        public async Task<Order> BuildNewOrderAsync(int paymentId)
        {
            var orderProducts = await BuildProductListFromCartAsync();
            var order = await ConstructOrderAsync(paymentId, orderProducts);
            return order;
        }

        private async Task<Order> ConstructOrderAsync(int paymentId, List<OrderProduct> orderProducts)
        {
            var userId = await _cookieHandler.GetClaimFromAuthenticationCookieAsync("UserId");
            var order = new Order()
            {
                UserId = Guid.Parse(userId),
                StatusId = 1,
                TotalPrice = _cartService.CalculateTotalPrice(),
                PaymentId = paymentId,
                Date = DateTime.Now,
                OrderProduct = orderProducts
            };

            return order;
        }

        private async Task<List<OrderProduct>> BuildProductListFromCartAsync()
        {
            var orderProducts = new List<OrderProduct>();

            foreach (var item in _cartService.GetCartContent())
            {
                orderProducts.Add(new OrderProduct
                {
                    ProductId = item.Product.Id,
                    Name = item.Product.Name,
                    Quantity = item.Quantity
                });
            }

            return await Task.FromResult(orderProducts);
        }
    }
}
