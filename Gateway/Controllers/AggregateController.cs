using Gateway.Models;
using Gateway.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregateController : ControllerBase
    {
        private ClientService _clientService;

        public AggregateController()
        {
            this._clientService = new ClientService();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync(Order order)
        {
            // Extract JWT token from Authorization header
            var authHeader = Request.Headers["Authorization"];

            // Send requests to Orders
            var orderResponse = await _clientService.PostRequestAsync("https://localhost:44398/api/Orders/create", order, authHeader);

            // Reduce payload to send. Only product Id and Quantity to remove is needed
            var updateStock = new Dictionary<Guid, int>();
            foreach(var item in order.OrderProduct)
                updateStock.Add(item.ProductId, item.Quantity);

            // Post request to Products and update products in stock
            var productResponse = await _clientService.PostRequestAsync("https://localhost:44339/api/products/updatestock", updateStock, authHeader);

            // If both responses are successful, return new order
            if (orderResponse.IsSuccessStatusCode && productResponse.IsSuccessStatusCode)
            {
                var json = await orderResponse.Content.ReadAsStringAsync();
                var newOrder = JsonConvert.DeserializeObject<Order>(json);

                return Ok(newOrder);
            }

            return null;
        }
    }
}
