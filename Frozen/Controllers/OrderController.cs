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
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IClientService _clientService;

        public OrderController(IClientService clientService)
        {
            this._clientService = clientService;
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
    }
}
