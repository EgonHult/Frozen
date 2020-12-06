using Frozen.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [HttpGet]
        public IActionResult RegisterPage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterPage(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //var response = GateWay.RegisterNewUser(viewModel);
            }
            return View();
        }
    }
}
