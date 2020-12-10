using Frozen.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJGaXJzdE5hbWUiOiJJY2VJY2UiLCJMYXN0TmFtZSI6IkJhYnkiLCJVc2VyRW1haWwiOiJhZG1pbkBmcm96ZW4uc2UiLCJVc2VySWQiOiIxNWNhM2YyZS0xZDhjLTQyYjItMjliYy0wOGQ4OTg3NmE5YWMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTYwNzU5OTUxNywiaXNzIjoiRnJvemVuIiwiYXVkIjoiRnJvemVuIn0.e5WM0kVNstRzrY5R2xGaxoNHuHDSspeodfKxFlFWeh8";
            
            // Act
            var result = tokenHandler.GetClaimsAsync(token).Result.ToList();

            var userId = result.Where(x => x.Type == "UserId").FirstOrDefault().Value;
            var userEmail = result.Where(x => x.Type == "UserEmail").FirstOrDefault().Value;

            // Assert
            Assert.AreEqual("15ca3f2e-1d8c-42b2-29bc-08d89876a9ac", userId);
            Assert.AreEqual("admin@frozen.se", userEmail);
        }

        [TestMethod]
        public void GetClaimsAsync_TryExtractFromNullStringCatchException_ReturnCatchedException()
        {
            try
            {
                // Arrange
                var tokenHandler = new JwtTokenHandler();
                var token = "";

                // Act
                var result = tokenHandler.GetClaimsAsync(token).Result;
            }
            catch(Exception)
            {
                // Success!
                return;
            }

            Assert.Fail("GetClaimsAsync() did not return any Exception!");
        }
    }
}
