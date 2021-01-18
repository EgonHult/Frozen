using Frozen.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Frozen.UnitTests
{
    [TestClass]
    public class JwtTokenHandlerTests
    {
        [TestMethod]
        public void GetClaimsAsync_ExtractClaimsFromJwtToken_ReturnDictionary()
        {
            // Arrange
            var tokenHandler = new JwtTokenHandler();
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJGaXJzdE5hbWUiOiJJY2VJY2UiLCJMYXN0TmFtZSI6IkJhYnkiLCJVc2VyRW1haWwiOiJhZG1pbkBmcm96ZW4uc2UiLCJVc2VySWQiOiJhMTVjNGU0My05Y2E1LTQ4MzktNWRlNC0wOGQ4OWQ0OGZmMzUiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL2V4cGlyYXRpb24iOiIyMDIwLTEyLTE1IDIwOjU4OjMyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2MDgwNjU5MTIsImlzcyI6IkZyb3plbiIsImF1ZCI6IkZyb3plbiJ9.teiWBhgdg4PxG1jAAVPykKHfJeV_rE74jiJymcpg5jU";
            
            // Act
            var result = tokenHandler.GetJwtTokenClaimsAsync(token).Result.ToList();

            var userId = result.Where(x => x.Type == "UserId").FirstOrDefault().Value;
            var userEmail = result.Where(x => x.Type == "UserEmail").FirstOrDefault().Value;

            // Assert
            Assert.AreEqual("a15c4e43-9ca5-4839-5de4-08d89d48ff35", userId);
            Assert.AreEqual("admin@frozen.se", userEmail);
        }

        [TestMethod]
        public void GetClaimsAsync_TryExtractFromNullJwtTokenCatchException_ReturnCatchedException()
        {
            try
            {
                // Arrange
                var tokenHandler = new JwtTokenHandler();
                string token = null;

                // Act
                var result = tokenHandler.GetJwtTokenClaimsAsync(token).Result;
            }
            catch(Exception)
            {
                // Success!
                return;
            }

            Assert.Fail("GetClaimsAsync() did not return any Exception!");
        }

        [TestMethod]
        public void ValidateJwtTokenLifeTimeAsync_ValidateExpirationDate_ReturnFalse()
        {
            // Arrange
            var tokenHandler = new JwtTokenHandler();
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJGaXJzdE5hbWUiOiJJY2VJY2UiLCJMYXN0TmFtZSI6IkJhYnkiLCJVc2VyRW1haWwiOiJhZG1pbkBmcm96ZW4uc2UiLCJVc2VySWQiOiJhMTVjNGU0My05Y2E1LTQ4MzktNWRlNC0wOGQ4OWQ0OGZmMzUiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL2V4cGlyYXRpb24iOiIyMDIwLTEyLTE1IDIwOjU4OjMyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2MDgwNjU5MTIsImlzcyI6IkZyb3plbiIsImF1ZCI6IkZyb3plbiJ9.teiWBhgdg4PxG1jAAVPykKHfJeV_rE74jiJymcpg5jU";

            // Act
            var result = tokenHandler.ValidateJwtTokenExpirationDateAsync(token).Result;

            // Assert
            Assert.IsFalse(result);
        }
    }
}
