using Frozen.Models;
using Frozen.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Frozen.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {

        private readonly UserManager<User> _userManager;

    
        public OrderController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult OrderRegistrationPage()
        {
            return View();
        }

        public async Task<IActionResult> OrderProductsPage()
        {
            var listOfProducts = new List<OrderProduct>
            {
                
                new OrderProduct { Id = new Guid(), Name = "Sak", OrderId = new Guid(), ProductId = new Guid(), Quantity= 1},
                new OrderProduct { Id = new Guid(), Name = "Grej", OrderId = new Guid(), ProductId = new Guid(), Quantity= 2},
            };
            var user = new User
            {
                Id = new Guid(), FirstName="FName", LastName ="LName", PhoneNumber = "0789123456", Email = "Test@gmail.com", Address="NewAddress", City="NewCity", Zip= "3746"
            };
            var order = new OrderResponse
            {
               Id = new Guid(), Date = DateTime.UtcNow, UserId = new Guid(), OrderProduct = listOfProducts, PaymentId = 32415, TotalPrice = 3452
               
            };
           
            var vm = new OrderViewModel
            {
                Order = order,

            };
           
            return View(order);
        }



       
       [HttpGet]
        public async Task<OrderResponse> GetOrderProducts()
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44350/order/orderregistration");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var order = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<OrderResponse>(order);
            
            }
        }
    }
}
