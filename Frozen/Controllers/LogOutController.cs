using Frozen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Frozen.Controllers
{
    public class LogOutController : Controller
    {
        private readonly CookieHandler _cookieHandler;

        public LogOutController(IHttpContextAccessor accessor)
        {
            this._cookieHandler = new CookieHandler(accessor);
        }

        [HttpGet]
        public IActionResult Index()
        {
            _cookieHandler.DestroyAllCookies();
            return RedirectToAction("Index", "Home");
        }
    }
}
