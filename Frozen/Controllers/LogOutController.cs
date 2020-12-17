using Frozen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Frozen.Controllers
{
    public class LogOutController : Controller
    {
        private readonly ICookieHandler _cookieHandler;

        public LogOutController(ICookieHandler cookieHandler)
        {
            this._cookieHandler = cookieHandler;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _cookieHandler.DestroyAllCookies();
            return RedirectToAction("Index", "Home");
        }
    }
}
