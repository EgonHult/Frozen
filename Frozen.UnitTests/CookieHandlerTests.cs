using Frozen.Services;
using Frozen.UnitTests.Sessions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
            var result = CookieHandler.GetSessionCookieContent(sessioncookieName);

            // Act
            Assert.AreEqual(content, result);
        }
    }
}
