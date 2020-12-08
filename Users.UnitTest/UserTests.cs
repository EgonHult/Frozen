using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                UnitTestContext.RoleManager);
            
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
                UnitTestContext.UserManager.DeleteAsync(createdUser).Wait();
        }
    }
}
