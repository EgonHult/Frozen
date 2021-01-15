using Frozen.Common;
using Frozen.Models;
using Frozen.Services;
using Frozen.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frozen.Controllers
{
    //[Authorize]
    public class OrderController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ICookieHandler _cookieHandler;
        private readonly CartService _cartService;

        public OrderController(IClientService clientService, CartService cartService, ICookieHandler cookieHandler)
        {
            _clientService = clientService;
            _cartService = cartService;
            _cookieHandler = cookieHandler;
        }
        public async Task<IActionResult> ViewOrderPage()
        {
            var userId = await _cookieHandler.GetClaimFromAuthenticationCookieAsync("UserId");
            var cart = _cartService.GetCart();

            var paymentResult = await _clientService.SendRequestToGatewayAsync(ApiLocation.Payments.GET_PAYMENTS, HttpMethod.Get);
            var paymentMethods = await _clientService.ReadResponseAsync<List<Payment>>(paymentResult.Content);

            var userResult = await _clientService.SendRequestToGatewayAsync(ApiLocation.Users.GET_USER + userId, HttpMethod.Get);
            var user = await _clientService.ReadResponseAsync<User>(userResult.Content);

            OrderViewModel vm = new OrderViewModel
            {
                User = user,
                PaymentMethods = paymentMethods,
                Cart = cart
            };
            ViewBag.TotalPrice = _cartService.CalculateTotalPrice();
            return View(vm);
        }
        [HttpGet]
        public IActionResult OrderRegistrationPage()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<List<Order>> GetOrdersAsync()
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Orders.ALL_ORDERS, HttpMethod.Get);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<List<Order>>(response.Content) : null;
        }

        [HttpGet]
        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Orders.GATEWAY_BASEURL + id, HttpMethod.Get);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<Order>(response.Content) : null;
        }

        [HttpPost]
        public async Task<Order> PostOrderAsync(Order order)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Orders.CREATE_ORDER, HttpMethod.Post, order);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<Order>(response.Content) : null;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<Order> UpdateOrderAsync(Guid id, Order order)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Orders.GATEWAY_BASEURL + id, HttpMethod.Put, order);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<Order>(response.Content) : null;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<Order> DeleteOrderAsync(Guid id)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Orders.GATEWAY_BASEURL + id, HttpMethod.Delete);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<Order>(response.Content) : null;
        }

        [HttpGet]
        public async Task<List<Order>> GetOrdersByUserIdAsync(Guid id)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Orders.GET_ORDER_BY_USERID + id, HttpMethod.Get);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<List<Order>>(response.Content) : null;
        }

        [HttpPost]
        public async Task<ActionResult> SendSwishRequest(int paymentId, string phoneNumber)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Payments.VERIFY_PAYMENT + paymentId, HttpMethod.Post, phoneNumber);
            
            if (response.IsSuccessStatusCode)
            {
                var orderResponse = await PostOrderToGatewayAsync(paymentId);

                if (orderResponse.IsSuccessStatusCode)
                    return Ok("Snygg mannen");
            }

            return BadRequest("Betalning kunde inte slutföras");
        }

        [HttpPost]
        public async Task<ActionResult> SendCardRequest(int paymentId, long cardNumber, int cvv, DateTime expiryDate)
        {
            CardModel card = new CardModel { CVV = cvv, ExpiryDate = expiryDate, Number = cardNumber, Id = paymentId, Type = "Bankkort" };

            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Payments.VERIFY_PAYMENT + paymentId, HttpMethod.Post, card);

            if (response.IsSuccessStatusCode)
            {
                var orderResponse = await PostOrderToGatewayAsync(paymentId);

                if (orderResponse.IsSuccessStatusCode)
                    return Ok("Snygg mannen");
            }

            return BadRequest("Betalning kunde inte slutföras");
        }

        [HttpPost]
        public async Task<ActionResult> SendInternetBankRequest(int paymentId, string bank)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Payments.VERIFY_PAYMENT + paymentId, HttpMethod.Post, bank);

            if (response.IsSuccessStatusCode)
            {
                var orderResponse = await PostOrderToGatewayAsync(paymentId);

                if (orderResponse.IsSuccessStatusCode)
                    return Ok("Snygg mannen");
            }

            return BadRequest("Betalning kunde inte slutföras");
        }

        private async Task<HttpResponseMessage> PostOrderToGatewayAsync(int paymentId)
        {
            var orderService = new OrderService(_cookieHandler, _cartService);

            var order           = await orderService.BuildNewOrderAsync(paymentId);
            var gatewayResponse = await _clientService.SendRequestToGatewayAsync(ApiLocation.Gateway.CREATE_ORDER, HttpMethod.Post, order);

            return gatewayResponse;
        }
    }
}
