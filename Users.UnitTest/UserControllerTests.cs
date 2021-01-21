using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Users.Models;
using Users.UnitTest.Context;
using Users.UnitTest.Fixture;

namespace Users.UnitTest
{
    [TestClass]
    public class UserControllerTests
    {
        // Admin tokens
        public const string validAdminToken10Years = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJGaXJzdE5hbWUiOiJJY2VJY2UiLCJMYXN0TmFtZSI6IkJhYnkiLCJVc2VyRW1haWwiOiJhZG1pbkBmcm96ZW4uc2UiLCJVc2VySWQiOiIxNWNhM2YyZS0xZDhjLTQyYjItMjliYy0wOGQ4OTg3NmE5YWMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL2V4cGlyYXRpb24iOiIyMDMxLTAxLTE5IDExOjQ1OjUwIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE5MjY1ODk1NTAsImlzcyI6IkZyb3plbiIsImF1ZCI6IkZyb3plbiJ9.m5VHiYBMvjDHBkmg4CfM3H9zfx53o7-0Xw-wBATCAPY";
        public const string validAdminRefreshToken10Years = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIxNWNhM2YyZS0xZDhjLTQyYjItMjliYy0wOGQ4OTg3NmE5YWMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL2V4cGlyYXRpb24iOiIyMDMxLTAxLTE5IDExOjQ3OjAxIiwiZXhwIjoxOTI2NTg5NjIxLCJpc3MiOiJGcm96ZW4iLCJhdWQiOiJGcm96ZW4ifQ.fovySCOniWBqcIdLisSIRKwmDbSbdHQIt7FPEWwacPc";

        public const string expiredAdminToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJGaXJzdE5hbWUiOiJJY2VJY2UiLCJMYXN0TmFtZSI6IkJhYnkiLCJVc2VyRW1haWwiOiJhZG1pbkBmcm96ZW4uc2UiLCJVc2VySWQiOiIxNWNhM2YyZS0xZDhjLTQyYjItMjliYy0wOGQ4OTg3NmE5YWMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL2V4cGlyYXRpb24iOiIyMDIxLTAxLTE5IDExOjQ4OjM4IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2MTEwNTY5MTgsImlzcyI6IkZyb3plbiIsImF1ZCI6IkZyb3plbiJ9.cFvfLj0kNHAWjuqtu4HI5N4mqaO55CERs7CS2gM2kN8";
        public const string expiredAdminRefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIxNWNhM2YyZS0xZDhjLTQyYjItMjliYy0wOGQ4OTg3NmE5YWMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL2V4cGlyYXRpb24iOiIyMDIxLTAxLTE5IDExOjQ4OjM4IiwiZXhwIjoxNjExMDU2OTE4LCJpc3MiOiJGcm96ZW4iLCJhdWQiOiJGcm96ZW4ifQ.B_aD622CXLwdKX-q-PPz3te8KBH5OgT4R10Rka9ayOM";

        // User tokens
        public const string validUserToken10Years = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJGaXJzdE5hbWUiOiJQdWxrYW4iLCJMYXN0TmFtZSI6IlBsw6R0dGVuIiwiVXNlckVtYWlsIjoidGVzdEBmcm96ZW4uc2UiLCJVc2VySWQiOiI3NzBmNzE0NS0zMGQyLTQ3NzktZjU0Zi0wOGQ4YmE2ZTFkNzciLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL2V4cGlyYXRpb24iOiIyMDMxLTAxLTE5IDExOjUzOjIxIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiVXNlciIsImV4cCI6MTkyNjU5MDAwMSwiaXNzIjoiRnJvemVuIiwiYXVkIjoiRnJvemVuIn0.SqlEsotbrDm0Dqjwlrj5yYAXCyc3UsKVkT8-E2Wc79Q";
        public const string validUserRefreshToken10Years = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiI3NzBmNzE0NS0zMGQyLTQ3NzktZjU0Zi0wOGQ4YmE2ZTFkNzciLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL2V4cGlyYXRpb24iOiIyMDMxLTAxLTE5IDExOjUzOjIxIiwiZXhwIjoxOTI2NTkwMDAxLCJpc3MiOiJGcm96ZW4iLCJhdWQiOiJGcm96ZW4ifQ.9eqJ-87TiKGPqFMs7hEvV5zTITOkfuG2fEktSloJB7I";

        public const string expiredUserToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJGaXJzdE5hbWUiOiJQdWxrYW4iLCJMYXN0TmFtZSI6IlBsw6R0dGVuIiwiVXNlckVtYWlsIjoidGVzdEBmcm96ZW4uc2UiLCJVc2VySWQiOiI3NzBmNzE0NS0zMGQyLTQ3NzktZjU0Zi0wOGQ4YmE2ZTFkNzciLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL2V4cGlyYXRpb24iOiIyMDIxLTAxLTE5IDExOjUxOjEwIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiVXNlciIsImV4cCI6MTYxMTA1NzA3MCwiaXNzIjoiRnJvemVuIiwiYXVkIjoiRnJvemVuIn0.sKJ2GCAUbuoktMnjzbr_sgsl8IRRd5FCt9SuD0iiNjQ";
        public const string expiredUserRefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiI3NzBmNzE0NS0zMGQyLTQ3NzktZjU0Zi0wOGQ4YmE2ZTFkNzciLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL2V4cGlyYXRpb24iOiIyMDIxLTAxLTE5IDExOjUxOjEwIiwiZXhwIjoxNjExMDU3MDcwLCJpc3MiOiJGcm96ZW4iLCJhdWQiOiJGcm96ZW4ifQ.DZFVVgmi1BWSdjPsOAFirrqmtHNJMrokkgxAZz2QeP0";

        public static UserFixture UserFixture { get; set; }
        public static TestUserContext UnitTestContext { get; private set; }
        public static User DummyUser { get; set; }
        public static UserModel DummyUserModel { get; set; }

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            // Set up DbContext
            UnitTestContext = new TestUserContext();

            // Arrange DummyUser for all tests
            UserFixture = new UserFixture(UnitTestContext.UserManager);
            DummyUser = UserFixture.CreateDummyUserAsync().Result;

            DummyUserModel = new UserModel()
            {
                Id = DummyUser.Id,
                FirstName = DummyUser.FirstName,
                LastName = DummyUser.LastName,
                Email = DummyUser.Email,
                PhoneNumber = DummyUser.PhoneNumber,
                Address = DummyUser.Address,
                City = DummyUser.City,
                Zip = DummyUser.Zip
            };
        }

        [ClassCleanup]
        public static void TestFixtureDispose()
        {
            UserFixture.CleanUpUserDatabaseBeforeTest().Wait();
        }

        [TestMethod]
        public void GetUsersAsync_GetAllUsers_ReturnSuccessFul()
        {
            using (var client = new HttpClient())
            {
                // Arrange
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44313/api/users/all");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", validAdminToken10Years);

                // Act
                var response = client.SendAsync(request).Result;

                // Assert
                Assert.IsTrue(response.IsSuccessStatusCode);
            }
        }

        [TestMethod]
        public void GetUsersAsync_GetAllUsersWithNoToken_ReturnFalse()
        {
            using (var client = new HttpClient())
            {
                // Arrange
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44313/api/users/all");

                // Act
                var response = client.SendAsync(request).Result;

                // Assert
                Assert.IsFalse(response.IsSuccessStatusCode);
            }
        }

        [TestMethod]
        public void GetUsersAsync_GetAllUsersWithExpiredToken_ReturnFalse()
        {
            using (var client = new HttpClient())
            {
                // Arrange
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44313/api/users/all");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", expiredAdminRefreshToken);

                // Act
                var response = client.SendAsync(request).Result;

                // Assert
                Assert.IsFalse(response.IsSuccessStatusCode);
            }
        }

        [TestMethod]
        public void GetUser_GetUserById_ReturnUser()
        {
            using (var client = new HttpClient())
            {
                // Arrange
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44313/api/users/getuserbyid/" + DummyUserModel.Id);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", validUserToken10Years);

                // Act
                var response = client.SendAsync(request).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                var user = JsonConvert.DeserializeObject<UserModel>(result);

                // Assert
                Assert.AreEqual(user.Id, DummyUserModel.Id);
            }
        }

        [TestMethod]
        public void GetUser_TryGetUserByIdWithEmptyId_ReturnBadRequest()
        {
            using (var client = new HttpClient())
            {
                // Arrange
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44313/api/users/getuserbyid/" + Guid.NewGuid());
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", validUserToken10Years);

                // Act
                var response = client.SendAsync(request).Result;

                // Assert
                Assert.IsFalse(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        public void PutUser_UpdateUserFirstName_ReturnUser()
        {
            using (var client = new HttpClient())
            {
                // Arrange
                var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:44313/api/users/" + DummyUser.Id);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", validUserToken10Years);

                DummyUserModel.FirstName = "Rocky";
                var json = JsonConvert.SerializeObject(DummyUserModel);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                // Act
                var response = client.SendAsync(request).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                var user = JsonConvert.DeserializeObject<UserModel>(result);

                // Assert
                Assert.AreEqual(user.FirstName, "Rocky");
            }
        }

        [TestMethod]
        public void PutUser_UpdateUserPhone_ReturnUser()
        {
            using (var client = new HttpClient())
            {
                // Arrange
                var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:44313/api/users/" + DummyUser.Id);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", validUserToken10Years);

                DummyUserModel.PhoneNumber = "070-123 45 67";
                var json = JsonConvert.SerializeObject(DummyUserModel);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                // Act
                var response = client.SendAsync(request).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                var user = JsonConvert.DeserializeObject<UserModel>(result);

                // Assert
                Assert.AreEqual(user.PhoneNumber, "070-123 45 67");
            }
        }

        [TestMethod]
        public void PutUser_UpdateUserPhone_ReturnBadRequest()
        {
            using (var client = new HttpClient())
            {
                // Arrange
                var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:44313/api/users/" + DummyUser.Id);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", validUserToken10Years);

                DummyUserModel.PhoneNumber = "070thrth eaggg reagreag";
                var json = JsonConvert.SerializeObject(DummyUserModel);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                // Act
                var response = client.SendAsync(request).Result;

                // Assert
                Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);

                // Restore dummy!
                DummyUserModel.PhoneNumber = DummyUser.PhoneNumber;
            }
        }

        [TestMethod]
        public void PutUser_TryUpdateUserWithExistingEmail_ReturnUnsuccessfulResponse()
        {
            using (var client = new HttpClient())
            {
                // Arrange
                var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:44313/api/users/" + DummyUserModel.Id);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", validUserToken10Years);

                DummyUserModel.Email = "admin@frozen.se";
                var json = JsonConvert.SerializeObject(DummyUserModel);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                // Act
                var response = client.SendAsync(request).Result;

                // Assert
                Assert.IsFalse(response.IsSuccessStatusCode);

                // Cleanup. Restore email!
                DummyUserModel.Email = DummyUser.Email;
            }
        }

        [TestMethod]
        public void PutUser_TryUpdateUserWithInvalidEmail_ReturnBadRequest()
        {
            using (var client = new HttpClient())
            {
                // Arrange
                var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:44313/api/users/" + DummyUserModel.Id);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", validUserToken10Years);

                DummyUserModel.Email = "45673 #7";
                var json = JsonConvert.SerializeObject(DummyUserModel);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                // Act
                var response = client.SendAsync(request).Result;

                // Assert
                Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);

                // Cleanup. Restore email!
                DummyUserModel.Email = DummyUser.Email;
            }
        }

        [TestMethod]
        public void RequestNewTokenAsync_RequestNewJwtToken_ReturnSuccessfulResponse()
        {
            using (var client = new HttpClient())
            {
                // Arrange
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44313/api/users/token");

                var tokenModel = new RenewTokenModel()
                {
                    UserId = DummyUserModel.Id,
                    Token = validUserRefreshToken10Years
                };

                var json = JsonConvert.SerializeObject(tokenModel);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                // Act
                var response = client.SendAsync(request).Result;

                // Assert
                Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            }
        }

        [TestMethod]
        public void RequestNewTokenAsync_RequestNewJwtTokenWithExpiredRefreshToken_ReturnUnsuccessfulResponse()
        {
            using (var client = new HttpClient())
            {
                // Arrange
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44313/api/users/token");

                var tokenModel = new RenewTokenModel()
                {
                    UserId = DummyUserModel.Id,
                    Token = expiredUserRefreshToken
                };

                var json = JsonConvert.SerializeObject(tokenModel);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                // Act
                var response = client.SendAsync(request).Result;

                // Assert
                Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
