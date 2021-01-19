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
            var cart = _cartService.GetCartContent();

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
            return await ReturnOrders(response);
        }

        [HttpGet]
        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Orders.GATEWAY_BASEURL + id, HttpMethod.Get);
            return await ReturnOrder(response);
        }

        [HttpPost]
        public async Task<Order> PostOrderAsync(Order order)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Orders.CREATE_ORDER, HttpMethod.Post, order);
            return await ReturnOrder(response);           
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<Order> UpdateOrderAsync(Guid id, Order order)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Orders.GATEWAY_BASEURL + id, HttpMethod.Put, order);
            return await ReturnOrder(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<Order> DeleteOrderAsync(Guid id)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Orders.GATEWAY_BASEURL + id, HttpMethod.Delete);
            return await ReturnOrder(response);
        }

        [HttpGet]
        public async Task<List<Order>> GetOrdersByUserIdAsync(Guid id)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Orders.GET_ORDER_BY_USERID + id, HttpMethod.Get);
            return await ReturnOrders(response);
        }    

        [HttpPost]
        public async Task<ActionResult> SendSwishRequest(int paymentId, string phoneNumber)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Payments.VERIFY_PAYMENT + paymentId, HttpMethod.Post, phoneNumber);
            return await PaymentResponse(paymentId, response);
        }

        [HttpPost]
        public async Task<ActionResult> SendCardRequest(string cardOwner, int paymentId, string cardNumber, int cvv, int month, int year)
        {
            long number = long.Parse(cardNumber.Replace(" ", ""));
            var expiryDate = DateTime.Parse($"{year}-{month}");

            CardModel card = new CardModel { CardOwner = cardOwner, CVV = cvv, ExpiryDate = expiryDate, Number = number, Id = paymentId, Type = "Bankkort" };

            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Payments.VERIFY_PAYMENT + paymentId, HttpMethod.Post, card);

            return await PaymentResponse(paymentId, response);
        }

        [HttpPost]
        public async Task<ActionResult> SendInternetBankRequest(int paymentId, string bank)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Payments.VERIFY_PAYMENT + paymentId, HttpMethod.Post, bank);
            return await PaymentResponse(paymentId, response);
        }

        private async Task<HttpResponseMessage> PostOrderToGatewayAsync(int paymentId)
        {
            var orderService = new OrderService(_cookieHandler, _cartService);

            var order           = await orderService.BuildNewOrderAsync(paymentId);
            var gatewayResponse = await _clientService.SendRequestToGatewayAsync(ApiLocation.Gateway.CREATE_ORDER, HttpMethod.Post, order);

            return gatewayResponse;
        }

        private async Task<ActionResult> PaymentResponse(int paymentId, HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var orderResponse = await PostOrderToGatewayAsync(paymentId);

                if (orderResponse.IsSuccessStatusCode)
                {
                    var newOrder = await _clientService.ReadResponseAsync<Order>(orderResponse.Content);
                    _cartService.EmptyCart();
                    return RedirectToAction("OrderConfirmationPage", new { orderId = newOrder.Id });
                }
            }

            return BadRequest("Betalning kunde inte slutföras");
        }

        private async Task<List<Order>> ReturnOrders(HttpResponseMessage response)
        {
            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<List<Order>>(response.Content) : null;
        }

        async Task<Order> ReturnOrder(HttpResponseMessage response)
        {
            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<Order>(response.Content) : null;
        }
        public async Task<ActionResult> OrderConfirmationPage(Guid orderId)
        {
            var userId = await _cookieHandler.GetClaimFromAuthenticationCookieAsync("UserId");
            var userResult = await _clientService.SendRequestToGatewayAsync(ApiLocation.Users.GET_USER + userId, HttpMethod.Get);
            var user = await _clientService.ReadResponseAsync<User>(userResult.Content);

            var orderResult = await _clientService.SendRequestToGatewayAsync(ApiLocation.Orders.GATEWAY_BASEURL + orderId, HttpMethod.Get);
            var order = await _clientService.ReadResponseAsync<Order>(orderResult.Content);

            OrderConfirmationViewModel Model = new OrderConfirmationViewModel
            {
                Order = order,
                User = user
            };
            return View(Model);
        }

    }
}
