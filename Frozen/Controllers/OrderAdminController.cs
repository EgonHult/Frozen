using Frozen.Common;
using Frozen.Models;
using Frozen.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frozen.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderAdminController : Controller
    {
        private readonly IClientService _clientService;

        public OrderAdminController(IClientService clientService)
        {
            this._clientService = clientService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _clientService.SendRequestToGatewayAsync(ApiLocation.Orders.ALL_ORDERS, HttpMethod.Get);

            if (result.IsSuccessStatusCode)
            {
                var orders = await _clientService.ReadResponseAsync<List<Order>>(result.Content);
                return View(orders);
            }

            return View();
        }

        public async Task<IActionResult> OrderUpdate(Guid id, bool updated = false)
        {
            if (id == Guid.Empty)
                return RedirectToAction(nameof(Index));

            var result = await _clientService.SendRequestToGatewayAsync(ApiLocation.Orders.GATEWAY_BASEURL + id, HttpMethod.Get);

            if (result.IsSuccessStatusCode)
            {
                var order = await _clientService.ReadResponseAsync<Order>(result.Content);

                if (updated)
                    ViewBag.OrderStatus = "Uppdaterad";

                return View(order);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OrderUpdate(Order order)
        {
            if (ModelState.IsValid)
            {
                var id = order.Id;
                var orderResult = await _clientService.SendRequestToGatewayAsync(ApiLocation.Orders.GATEWAY_BASEURL + order.Id, HttpMethod.Get);
                
                if(orderResult.IsSuccessStatusCode)
                {
                    var orderToUpdate = await _clientService.ReadResponseAsync<Order>(orderResult.Content);
                    orderToUpdate.StatusId = order.StatusId;

                    var result = await _clientService.SendRequestToGatewayAsync(ApiLocation.Orders.GATEWAY_BASEURL + id, HttpMethod.Put, orderToUpdate);

                    if (result.IsSuccessStatusCode)
                    {
                        var updatedOrder = await _clientService.ReadResponseAsync<Order>(result.Content);
                        return RedirectToAction(nameof(OrderUpdate), new { id = updatedOrder.Id, updated=true });
                    }
                }
            }

            return View();
        }
    }
}
