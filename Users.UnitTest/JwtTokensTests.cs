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
        public void CreateToken_TryCreateNewJwtToken_ReturnNotNULL()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var user = DummyUser.TestUser2();

            // Act
            string token = tokenHandler.CreateToken(user);

            // Assert
            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void CreateToken_TryCreateNewJwtTokenWithNullUser_ReturnNULL()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var user = DummyUser.TestUser();

            // Act
            string token = tokenHandler.CreateToken(null);

            // Assert
            Assert.IsNull(token);
        }

        [TestMethod]
        public void CreateRefreshToken_TryCreateNewRefreshJwtToken_ReturnNotNULL()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var user = DummyUser.TestUser2();

            // Act
            string token = tokenHandler.CreateRefreshToken(user);

            // Assert
            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void CreateRefreshToken_TryCreateNewRefreshJwtTokenWithNullUser_ReturnNULL()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var user = DummyUser.TestUser();

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
            var user = DummyUser.TestUser2();
            string token = tokenHandler.CreateToken(user);

            // Act
            var result = tokenHandler.ValidateToken(token)
                .Identity
                .IsAuthenticated;

            // Assert
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void ValidateRefreshToken_CheckIfJwtRefreshTokenIsValid_ReturnTrue()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var user = DummyUser.TestUser2();
            string token = tokenHandler.CreateRefreshToken(user);

            // Act
            var result = tokenHandler.ValidateRefreshToken(token)
                .Identity
                .IsAuthenticated;

            // Assert
            Assert.IsTrue(result);

        }
    }
}
