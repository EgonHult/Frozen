using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Users.Models;
using Users.Repositories;
using Users.UnitTest.Context;
using Users.UnitTest.Fixture;

namespace Users.UnitTest
{
    [TestClass]
    public class UserTests
    {
        public static UserRepository UserRepositoryClass { get; set; }
        public static TestUserContext UnitTestContext { get; set; }
        public static User FixtureUser { get; set; }

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            // Set up DbContext and IdentityFramework
            UnitTestContext = new TestUserContext();

            UserRepositoryClass = new UserRepository(
                UnitTestContext.UserDbContext, 
                UnitTestContext.UserManager, 
                UnitTestContext.SignInManager, 
                UnitTestContext.RoleManager,
                UnitTestContext.JwtTokenHandler);
            
            // Register a temporary test-user in database for unittesting
            var userFixture = new UserFixture(UnitTestContext.UserManager);
            FixtureUser = userFixture.CreateDummyUserAsync().Result;
        }

        [ClassCleanup]
        public static void TestFixtureDispose()
        {
            // Remove test-user from database
            var userFixture = new UserFixture(UnitTestContext.UserManager);
            userFixture.RemoveDummyUserAsync(FixtureUser).Wait();

            UnitTestContext.Dispose();
        }
        
        [TestMethod]
        public void CreateUser_RegisterNewUser_ReturnCreatedUserAreEqual()
        {
            // Arrange
            var userModel = DummyUser.TestUserModel();

            // Act
            var createdUser = UserRepositoryClass.CreateUserAsync(userModel).Result;

            // Assert
            Assert.IsNotNull(createdUser);

            // Clean up and delete createdUser!
            if (createdUser != null)
            {
                var user = UnitTestContext.UserManager.FindByEmailAsync(createdUser.Email).Result;
                UnitTestContext.UserManager.DeleteAsync(user).Wait();
            }
        }

        [TestMethod]
        public void GetUserByIdAsync_GetUserById_ReturnUser()
        {
            // Arrange
            var fixtureUser = FixtureUser;

            // Act
            var user = UserRepositoryClass.GetUserByIdAsync(fixtureUser.Id).Result;

            // Assert
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void GetAllUsersAsync_GetAllUsersFromDatase_ReturnListOfUsers()
        {
            // Act
            var users = UserRepositoryClass.GetAllUsersAsync().Result;

            // Assert
            Assert.IsInstanceOfType(users, typeof(List<UserModel>));
        }

        [TestMethod]
        public void DeleteUserAsync_DeleteUserById_ReturnDeletedUser()
        {

            // Arrange
            var userModel = DummyUser.TestUserModel();
            var createdUser = UserRepositoryClass.CreateUserAsync(userModel).Result;
            var user = UnitTestContext.UserManager.FindByEmailAsync(createdUser.Email).Result;

            // Act
            var deletedUser = UserRepositoryClass.DeleteUserAsync(user.Id).Result;

            // Assert
            Assert.AreEqual(user.Id, deletedUser.Id);
            
        }

        [TestMethod]
        public void UpdateUserAsync_UpdateUserName_ReturnUpdatedUser()
        {
            // Arrange
            var userModel = new UserModel()
            {
                Id = FixtureUser.Id,
                FirstName = "Big",
                LastName = "Mac",
                Email = FixtureUser.Email,
                PhoneNumber = FixtureUser.PhoneNumber,
                Address = FixtureUser.Address,
                City = FixtureUser.City,
                Zip = FixtureUser.Zip
            };

            // Act
            var updatedUser = UserRepositoryClass.UpdateUserAsync(FixtureUser.Id, userModel).Result;

            // Assert
            Assert.AreEqual(userModel.FirstName, updatedUser.FirstName);
            Assert.AreEqual(userModel.LastName, updatedUser.LastName);
        }

        [TestMethod]
        public void LoginUserAsync_LoginUser_ReturnUserAndTokens()
        {
            // Arrange
            var loginModel = new LoginModel()
            {
                Username = "testuser@frozen.se",
                Password = "Test123!",
            };

            // Act
            var response = UserRepositoryClass.LoginUserAsync(loginModel).Result;

            // Assert
            Assert.IsNotNull(response.User);
            Assert.IsNotNull(response.Token);
            Assert.IsNotNull(response.RefreshToken);
        }

        [TestMethod]
        public void LoginUserAsync_LoginWithWrongCredentials_ReturnNull()
        {
            // Arrange
            var loginModel = new LoginModel()
            {
                Username = "eddie666@metalhead.com",
                Password = "numberofthebeast",
            };

            // Act
            var response = UserRepositoryClass.LoginUserAsync(loginModel).Result;

            // Assert
            Assert.IsNull(response);
        }

        [TestMethod]
        public void UpdateEmailAddressAsync_UpdateUserEmail_ReturnUpdatedUser()
        {
            // Arrange
            var oldEmail = FixtureUser.Email;
            var newEmail = "starchild@galaxy.com";

            var userModel = new UserModel()
            {
                Id = FixtureUser.Id,
                FirstName = FixtureUser.FirstName,
                LastName = FixtureUser.LastName,
                Email = newEmail,
                PhoneNumber = FixtureUser.PhoneNumber,
                Address = FixtureUser.Address,
                City = FixtureUser.City,
                Zip = FixtureUser.Zip
            };

            // Act
            var updatedUser = UserRepositoryClass.UpdateEmailAddressAsync(userModel).Result;

            // Assert
            Assert.AreEqual(userModel.Email, updatedUser.Email);
            Assert.AreNotEqual(oldEmail, updatedUser.Email);
        }

        [TestMethod]
        public void UpdatePasswordAsync_UpdateUserPassword_ReturnUpdatedUser()
        {
            // Arrange
            var userId = FixtureUser.Id;
            var oldPass = "Test123!";
            var newPass = "CaptenAmazing123!";

            // Act
            var result = UserRepositoryClass.UpdatePasswordAsync(FixtureUser.Id, oldPass, newPass).Result;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
