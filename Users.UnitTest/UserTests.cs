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
            UnitTestContext = new TestUserContext();

            UserRepositoryClass = new UserRepository(
                UnitTestContext.UserDbContext, 
                UnitTestContext.UserManager, 
                UnitTestContext.SignInManager, 
                UnitTestContext.RoleManager);
            
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

            // Clean up and delete test-user from database!
            UnitTestContext.UserManager.DeleteAsync(createdUser).Wait();
        }
    }
}
