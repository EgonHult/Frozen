using Frozen.Common;
using Frozen.Models;
using Frozen.Services;
using Frozen.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frozen.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ICookieHandler _cookieHandler;
        private readonly IClientService _clientService;
        private readonly string jwtToken;
        public OrderController(ICookieHandler cookieHandler, IClientService clientService)
        {
            this._cookieHandler = cookieHandler;
            this._clientService = clientService;
            jwtToken = _cookieHandler.ReadSessionCookieContent(Cookies.JWT_SESSION_TOKEN);

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
            var apiLocation = "https://localhost:44350/order/getall";
            var response = await _clientService.SendRequestToGatewayAsync(apiLocation, HttpMethod.Get, null, jwtToken);
            response.EnsureSuccessStatusCode();
            var orders = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Order>>(orders);
        }
        [HttpGet]
        public async Task<Order> GetOrderByIdAsync(Guid Id)
        {
            var apiLocation = "https://localhost:44350/order/" + Id;
            var response = await _clientService.SendRequestToGatewayAsync(apiLocation, HttpMethod.Get, null, jwtToken);
            response.EnsureSuccessStatusCode();
            var order = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Order>(order);
        }
        [HttpPost]
        public async Task<Order> PostOrderAsync(Order order)
        {
            var apiLocation = "https://localhost:44350/order/create";
            var response = await _clientService.SendRequestToGatewayAsync(apiLocation, HttpMethod.Post, order, jwtToken);
            response.EnsureSuccessStatusCode();
            var createdOrder = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Order>(createdOrder);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<Order> UpdateOrderAsync(Guid Id, Order order)
        {
            var apiLocation = "https://localhost:44350/order/" + Id;
            var response = await _clientService.SendRequestToGatewayAsync(apiLocation, HttpMethod.Put, order, jwtToken);
            response.EnsureSuccessStatusCode();
            var UpdatedOrder = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Order>(UpdatedOrder);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<Order> DeleteOrderAsync(Guid Id)
        {
            var apiLocation = "https://localhost:44350/order/" + Id;
            var response = await _clientService.SendRequestToGatewayAsync(apiLocation, HttpMethod.Delete, null, jwtToken);
            response.EnsureSuccessStatusCode();
            var order = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Order>(order);
        }
        [HttpGet]
        public async Task<List<Order>> GetOrdersByUserIdAsync(Guid Id)
        {
            var apiLocation = "https://localhost:44350/order/user/" + Id;
            var response = await _clientService.SendRequestToGatewayAsync(apiLocation, HttpMethod.Get, null, jwtToken);
            response.EnsureSuccessStatusCode();
            var orders = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Order>>(orders);
        }
    }
}
