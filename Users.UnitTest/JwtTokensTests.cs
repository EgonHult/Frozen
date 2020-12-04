using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Users.Models;
using Users.Services;

namespace Users.UnitTest
{
    [TestClass]
    public class JwtTokensTests
    {
        [TestMethod]
        public void CreateToken_CreateNewJwtToken_ReturnTokenNotNULL()
        {
            // Arrange
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            JwtTokenHandler createToken = new JwtTokenHandler(config);
            var user = DummyUser.User();

            // Act
            string token = createToken.CreateToken(user);

            // Assert
            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void SetTokenClaims_CreateSetOfClaims_ReturnItemsNotNull()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler();
            var user = DummyUser.User();

            // Act
            var result = tokenHandler.SetTokenClaims(user).ToList();

            // Assert
            CollectionAssert.AllItemsAreNotNull(result);
        }

        [TestMethod]
        public void ConfigureToken_CreateJwtSecurityToken_ReturnJwtSecurityTokenNotNull()
        {
            // Arrange
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtTokenHandler tokenHandler = new JwtTokenHandler(config);
            var user = DummyUser.User();
            var claims = tokenHandler.SetTokenClaims(user);

            // Act
            var result = tokenHandler.ConfigureToken(claims, credentials);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
