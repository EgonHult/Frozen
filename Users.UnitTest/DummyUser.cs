using System;
using Users.Models;

namespace Users.UnitTest
{
    public static class DummyUser
    {
        public static RegisterUserModel TestUserModel()
        {
            RegisterUserModel  user = new RegisterUserModel()
            {
                FirstName = "UnitTest",
                LastName = "UnitTest",
                Address = "UnitTest 123",
                City = "UnitTest",
                Zip = "12345",
                PhoneNumber = "070-123 45 67",
                Email = "unittest@frozen.se",
                Password = "Test123!"
            };

            return user;
        }

        public static User TestUser()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "Test",
                Address = "Test 123",
                City = "Test",
                Zip = "12345",
                PhoneNumber = "070-123 45 67",
                Email = "testuser@frozen.se",
                UserName = "testuser@frozen.se"
            };

            return user;
        }

    }
}
