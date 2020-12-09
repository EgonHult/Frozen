using Frozen.Models;
using Frozen.ViewModels;
using Microsoft.AspNetCore.Http;
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
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginPageAsync(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44350/user/login");
                    string json = JsonConvert.SerializeObject(viewModel);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.SendAsync(request);
                    var responseMessage = await response.Content.ReadAsStringAsync();
                    if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        ViewBag.Message = "Felaktiga inloggningsuppgifter, Loser";
                    }
                    else if(response.IsSuccessStatusCode)
                    {
                        LoggedInUser loggedInUser = JsonConvert.DeserializeObject<LoggedInUser>(responseMessage);
                        //ViewBag.Message = "Du är nu inloggad, " + loggedInUser.User.FirstName;
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View("LoginPage");
        }
        [HttpGet]
        public IActionResult RegisterPage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterPageAsync(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44350/user/register");
                    string json = JsonConvert.SerializeObject(viewModel);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.SendAsync(request);
                    var responseMessage = response.Content;
                    if(response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ViewBag.Exists = "Användare med Email " + viewModel.Email + " finns redan!";
                    }
                    else if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("LoginPage");
                    }
                }

            }
            return View();
        }
    }
}
