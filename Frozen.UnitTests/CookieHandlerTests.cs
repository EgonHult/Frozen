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
        public static CookieHandler CookieHandler { get; set; }

        [ClassInitialize]
        public static void LoadAppsettings(TestContext context)
        {
            var config = new HttpContextConfig();
            var httpContext = config.HttpContext;
            CookieHandler = new CookieHandler(httpContext);
        }

        [TestMethod]
        public void CreateSessionCookie_TryCreateNewSessionCookie_ReturnEqualSessionContent()
        {
            // Arrange
            var sessioncookieName = "UnitTestSessionCookie";
            var content = Guid.NewGuid().ToString();

            // Act
            CookieHandler.CreateSessionCookie(sessioncookieName, content);
            var result = CookieHandler.ReadSessionCookieContent(sessioncookieName);

            // Act
            Assert.AreEqual(content, result);
        }
    }
}
