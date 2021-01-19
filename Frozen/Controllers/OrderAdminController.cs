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

        public async Task<IActionResult> OrderUpdate(Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToAction(nameof(Index));

            var result = await _clientService.SendRequestToGatewayAsync(ApiLocation.Orders.GATEWAY_BASEURL + id, HttpMethod.Get);

            if (result.IsSuccessStatusCode)
            {
                var order = await _clientService.ReadResponseAsync<Order>(result.Content);
                return View(order);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OrderUpdate(int statusId, Guid id)
        {
            var apiLocation = ApiLocation.Orders.GATEWAY_BASEURL + $"{statusId}/{id}";
            var response = await _clientService.SendRequestToGatewayAsync(apiLocation, HttpMethod.Put);

            var updateSuccessFlag = false;

            if (response.IsSuccessStatusCode)
                updateSuccessFlag = true;

            TempData["StatusUpdated"] = updateSuccessFlag;
            TempData["OrderID"] = id;
            return RedirectToAction("Index");
        }
    }
}
