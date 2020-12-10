using Frozen.Services;
using Frozen.UnitTests.Sessions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frozen.UnitTests
{
    [TestClass]
    public class CookieHandlerTests
    {
        public static IHttpContextAccessor HttpContext { get; set; }

        [ClassInitialize]
        public static void LoadAppsettings(TestContext context)
        {
            var config = new HttpContextConfig();
            HttpContext = config.HttpContext;
        }

        [TestMethod]
        public void CreateSessionCookie_TryCreateNewSessionCookie_ReturnCreatedCookie()
        {
            // Arrange
            var cookieHandler = new CookieHandler(HttpContext);
            var sessioncookieName = "UnitTestSessionCookie";
            var content = Guid.NewGuid().ToString();

            // Act
            cookieHandler.CreateSessionCookie(sessioncookieName, content);
            var result = cookieHandler.ReadSessionCookieContent(sessioncookieName);

            // Act
            Assert.AreEqual("some content here", result);
        }
    }
}
