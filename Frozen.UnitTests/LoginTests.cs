using Frozen.Controllers;
using Frozen.Services;
using Frozen.UnitTests.Sessions;
using Frozen.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Frozen.UnitTests
{
    [TestClass]
    public class LoginTests
    {
        private static LoginController LoginController { get; set; }

        [ClassInitialize]
        public static void LoadAppsettings(TestContext context)
        {
            var config = new HttpContextConfig();

            IHttpContextAccessor httpContext = config.HttpContext;
            ICookieHandler _cookieHandler = new CookieHandler(httpContext);
            IClientService clientService = new ClientService(_cookieHandler);

            LoginController = new LoginController(_cookieHandler, clientService);
        }

        [TestMethod]
        public void LoginPageAsync_LoginUser_ReturnsRedirectToHomePage()
        {
            //Arrange
            LoginViewModel viewModel = new LoginViewModel
            {
                Username = "admin@frozen.se",
                Password = "Test123!"
            };

            //Act
            var response = LoginController.LoginPageAsync(viewModel).Result as RedirectToActionResult;

            //Assert
            Assert.AreEqual("Index", response.ActionName);
            Assert.AreEqual("Home", response.ControllerName);
        }

        [TestMethod]
        public void LoginPageAsync_LoginUser_ReturnsLoginViewWithError()
        {
            //Arrange
            LoginViewModel viewModel = new LoginViewModel
            {
                Username = "admin@frozen.se",
                Password = "!321tseT"
            };

            //Act
            var response = LoginController.LoginPageAsync(viewModel).Result as ViewResult;

            //Assert
            Assert.AreEqual("LoginPage", response.ViewName);
            Assert.AreEqual("Felaktiga inloggningsuppgifter", LoginController.ViewBag.Message);
        }
    }
}
