using Frozen.Controllers;
using Frozen.UnitTests.Sessions;
using Frozen.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Frozen.UnitTests
{
    [TestClass]
    public class LoginTests
    {
        private static IHttpContextAccessor HttpContext { get; set; }
        private static LoginController LoginController { get; set; }

        [ClassInitialize]
        public static void LoadAppsettings(TestContext context)
        {
            var config = new HttpContextConfig();
            HttpContext = config.HttpContext;

            // Instantiate a new LoginController object to test on
            LoginController = new LoginController(HttpContext);
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
            Assert.AreEqual("Felaktiga inloggningsuppgifter, Loser", LoginController.ViewBag.Message);
        }
    }
}
