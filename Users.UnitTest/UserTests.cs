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
        public static IUserRepository UserRepositoryClass { get; set; }
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
        public void CreateUserAsync_RegisterNewUser_ReturnCreatedUser()
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
        public void CreateUserAsync_TryRegisterNewUserWithEmptyModel_ReturnException()
        {
            // Arrange
            var userModel = new RegisterUserModel();

            try
            {
                // Act
                var createdUser = UserRepositoryClass.CreateUserAsync(userModel).Result;
            }
            catch(Exception)
            {
                // Success!
                return;
            }
        }

        [TestMethod]
        public void CreateUserAsync_TryRegisterNewUserWithExistingEmail_ReturnNull()
        {
            // Arrange
            var newUser = new RegisterUserModel()
            {
                FirstName = "Test",
                LastName = "Test",
                Address = "Test",
                City = "Test",
                Zip = "12345",
                PhoneNumber = "+46-12-3456789",
                Email = "testuser@frozen.se",
                Password = "Test123!"
            };

            // Act
            var createdUser = UserRepositoryClass.CreateUserAsync(newUser).Result;

            // Assert
            Assert.IsNull(createdUser);
        }

        [TestMethod]
        public void CreateUserAsync_TryRegisterNewUserWithInvalidPasswordFormat_ReturnNull()
        {
            // Arrange
            var newUser = new RegisterUserModel()
            {
                FirstName = "Test",
                LastName = "Test",
                Address = "Test",
                City = "Test",
                Zip = "12345",
                PhoneNumber = "+46-12-3456789",
                Email = "someotheremail@frozen.se",
                Password = "123fdghfghfgh8"
            };

            // Act
            var createdUser = UserRepositoryClass.CreateUserAsync(newUser).Result;

            // Assert
            Assert.IsNull(createdUser);
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
        public void GetUserByIdAsync_TryGetUserById_ReturnNull()
        {
            //Arange
            var notExistingUserId = Guid.NewGuid();

            //Act
            var user = UserRepositoryClass.GetUserByIdAsync(notExistingUserId).Result;

            //Assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetAllUsersAsync_GetAllUsersFromDatabase_ReturnListOfUsers()
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
        public void DeleteUserAsync_DeleteUserByEmptyGuidId_ReturnNull()
        {
            // Arrange
            var emptyUserId = Guid.Empty;

            // Act
            var result = UserRepositoryClass.DeleteUserAsync(emptyUserId).Result;

            // Assert
            Assert.IsNull(result);

        }

        [TestMethod]
        public void DeleteUserAsync_TryDeleteUserNonExistingUserId_ReturnNull()
        {
            // Arrange
            var nonexistingUserId = Guid.NewGuid();

            // Act
            var result = UserRepositoryClass.DeleteUserAsync(nonexistingUserId).Result;

            // Assert
            Assert.IsNull(result);

        }

        [TestMethod]
        public void UpdateUserAsync_UpdateUserFirstNameAndLastName_ReturnUpdatedUser()
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
            var updatedUser = UserRepositoryClass.UpdateUserAsync(userModel.Id, userModel).Result;

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

            // Restore testuser
            updatedUser.Email = "testuser@frozen.se";
            var tmp = UserRepositoryClass.UpdateEmailAddressAsync(updatedUser).Result;
        }

        [TestMethod]
        public void UpdateEmailAddressAsync_TryUpdateUserEmailToAlreadyExistingEmail_ReturnNull()
        {
            // Arrange
            var existingEmail = "admin@frozen.se";

            var userModel = new UserModel()
            {
                Id = FixtureUser.Id,
                FirstName = FixtureUser.FirstName,
                LastName = FixtureUser.LastName,
                Email = existingEmail,
                PhoneNumber = FixtureUser.PhoneNumber,
                Address = FixtureUser.Address,
                City = FixtureUser.City,
                Zip = FixtureUser.Zip
            };

            // Act
            var result = UserRepositoryClass.UpdateEmailAddressAsync(userModel).Result;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void UpdatePasswordAsync_UpdateUserPassword_ReturnUpdatedUser()
        {
            // Arrange
            var userId = FixtureUser.Id;
            var oldPass = "Test123!";
            var newPass = "CaptenAmazing123!";

            // Act
            var result = UserRepositoryClass.UpdatePasswordAsync(userId, oldPass, newPass).Result;

            // Assert
            Assert.IsNotNull(result);

            // Restore password
            var tmp = UserRepositoryClass.UpdatePasswordAsync(userId, newPass, oldPass).Result;
        }

        [TestMethod]
        public void CheckIfUserExistsByEmailAsync_CheckIfNonExistingEmailIsRegistered_ReturnFalse()
        {
            // Arrange
            var emailToTest = "funkydude666@frozen.se";

            // Act
            var result = UserRepositoryClass.CheckIfUserExistsByEmailAsync(emailToTest).Result;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckIfUserExistsByEmailAsync_CheckAlreadyRegisteredEmail_ReturnTrue()
        {
            // Arrange
            var emailToTest = "testuser@frozen.se";

            // Act
            var result = UserRepositoryClass.CheckIfUserExistsByEmailAsync(emailToTest).Result;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateNewTokensAsync_RequestNewTokens_ReturnTokenModelNotNull()
        {
            // Arrange
            var loginModel = new LoginModel()
            {
                Username = "testuser@frozen.se",
                Password = "Test123!",
            };

            var response = UserRepositoryClass.LoginUserAsync(loginModel).Result;

            // Act
            var result = UserRepositoryClass.GenerateNewTokensAsync(response.User.Id, response.RefreshToken).Result;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
