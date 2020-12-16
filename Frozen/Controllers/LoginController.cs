using Frozen.Common;
using Frozen.Models;
using Frozen.Services;
using Frozen.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frozen.Controllers
{
    public class LoginController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ICookieHandler _cookieHandler;

        public LoginController(ICookieHandler cookieHandler, IClientService clientService)
        {
            this._cookieHandler = cookieHandler;
            this._clientService = clientService;
        }

        [HttpGet]
        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterPage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginPageAsync(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Users.LOGIN_ENDPOINT, HttpMethod.Post, viewModel);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ViewBag.Message = "Felaktiga inloggningsuppgifter, Loser";
                }
                else if (response.IsSuccessStatusCode)
                {
                    var loggedInUser = await _clientService.ReadResponseAsync<LoggedInUser>(response.Content);
                    await _cookieHandler.CreateLoginCookiesAsync(loggedInUser);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View("LoginPage");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPageAsync(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _clientService.SendRequestToGatewayAsync(ApiLocation.Users.REGISTER_ENDPOINT, HttpMethod.Post, viewModel);

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    ViewBag.Exists = "Användare med Email " + viewModel.Email + " finns redan!";
                }
                else if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("LoginPage");
                }
            }

            return View();
        }
    }
}