using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Users.Models;
using Users.Repositories;
using Users.UnitTest.Context;

namespace Users.UnitTest
{
    [TestClass]
    public class UserTests
    {
        public static UserModel TestDummyUser { get; set; }

        [ClassInitialize]
        public static void LoadAppsettings(TestContext context)
        {
            TestDummyUser = DummyUser.TestUser();
        }

        [TestMethod]
        public void CreateUser_RegisterNewUser_ReturnCreatedUserIsNotNull()
        {
            using(var context = new TestUsersDbContext())
            {
                // Arrange
                //var userManager = new UserManager<User>(options => { options. });
                var userRepository = new UserRepository(context.DbContext, context.UserManager);

                // Act
                var createdUser = userRepository.CreateUserAsync(TestDummyUser);

                // Assert
                Assert.IsNotNull(createdUser.Result);
            }
        }
    }
}
