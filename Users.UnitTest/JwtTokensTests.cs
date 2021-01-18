using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Users.Models;
using Users.Services;
using Users.UnitTest.Context;

namespace Users.UnitTest
{
    [TestClass]
    public class JwtTokensTests
    {
        public static IConfigurationRoot Config { get; set; }

        /// <summary>
        /// Executes once before ALL tests
        /// </summary>
        /// <param name="context"></param>
        [ClassInitialize]
        public static void LoadAppsettings(TestContext context)
        {
            var appSettings = new AppSettings();
            Config = appSettings.Config;
        }

        [TestMethod]
        public void CreateToken_CreateNewJwtToken_ReturnJwtTokenString()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var user = DummyUser.TestUser();

            // Act
            string token = tokenHandler.CreateToken(user, false);

            // Assert
            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void CreateToken_TryCreateNewJwtTokenWithNullUser_ReturnNULL()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var user = DummyUser.TestUserModel();

            // Act
            string token = tokenHandler.CreateToken(null, false);

            // Assert
            Assert.IsNull(token);
        }

        [TestMethod]
        public void CreateRefreshToken_TryCreateNewRefreshJwtToken_ReturnNotNull()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var user = DummyUser.TestUser();

            // Act
            string token = tokenHandler.CreateRefreshToken(user);

            // Assert
            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void CreateRefreshToken_TryCreateNewRefreshJwtTokenWithNullUser_ReturnNull()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var user = DummyUser.TestUserModel();

            // Act
            string token = tokenHandler.CreateRefreshToken(null);

            // Assert
            Assert.IsNull(token);
        }

        [TestMethod]
        public void ValidateToken_CheckIfJwtTokenIsValid_ReturnTrue()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var user = DummyUser.TestUser();
            string token = tokenHandler.CreateToken(user, false);

            // Act
            var result = tokenHandler.ValidateToken(token);

            // Assert
            Assert.IsTrue(result.Identity.IsAuthenticated);
        }

        [TestMethod]
        public void ValidateRefreshToken_CheckIfJwtRefreshTokenIsValid_ReturnTrue()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var user = DummyUser.TestUser();
            string token = tokenHandler.CreateRefreshToken(user);

            // Act
            var result = tokenHandler.ValidateRefreshToken(token)
                .Identity
                .IsAuthenticated;

            // Assert
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void ValidateRefreshToken_ValidateExpiredJwtToken_ReturnFalse()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var expiredToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJGaXJzdE5hbWUiOiJUZXN0IiwiTGFzdE5hbWUiOiJUZXN0IiwiVXNlckVtYWlsIjoidGVzdHVzZXJAZnJvemVuLnNlIiwiVXNlcklkIjoiYmU1Y2I5YmEtNjFlOC00YzI4LWIwYzMtOGQ1MTNlMDE4ZDVmIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9leHBpcmF0aW9uIjoiMjAyMS0wMS0xOCAwMDoxNTo0OSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJleHAiOjE2MTA5MjUzNDksImlzcyI6IkZyb3plbiIsImF1ZCI6IkZyb3plbiJ9.WxUmqhakf-wN_4HOVTi4_bitrlU5CyLc2LShEzXhuTM";

            // Act
            var result = tokenHandler.ValidateToken(expiredToken);

            // Assert
            Assert.IsNull(result);
        }
    }
}
