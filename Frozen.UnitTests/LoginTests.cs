using Frozen.Controllers;
using Frozen.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frozen.UnitTests
{
    [TestClass]
    public class LoginTests
    {
        [TestMethod]
        public void LoginPageAsync_LoginUser_ReturnsRedirectToHomePage()
        {
            //Arrange
            LoginViewModel viewModel = new LoginViewModel
            {
                Username = "admin@frozen.se",
                Password = "Test123!"
            };
            LoginController controller = new LoginController();
            //Act
            var response = controller.LoginPageAsync(viewModel).Result as RedirectToActionResult;
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
            LoginController controller = new LoginController();
            //Act
            var response = controller.LoginPageAsync(viewModel).Result as ViewResult;
            //Assert
            Assert.AreEqual("LoginPage", response.ViewName);
            Assert.AreEqual("Felaktiga inloggningsuppgifter, Loser", controller.ViewBag.Message);
        }
    }
}
