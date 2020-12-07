using System;
using Users.Models;

namespace Users.UnitTest
{
    public static class DummyUser
    {
        public static UserModel TestUserModel()
        {
            UserModel  user = new UserModel()
            {
                FirstName = "Joel",
                LastName = "Felldin",
                Address = "Alfred Nobels Allé",
                City = "Huddinge",
                Zip = "14152",
                PhoneNumber = "07012312312",
                Email = "joel.felldin@iths.se",
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
                Address = "Test",
                City = "Test",
                Zip = "12345",
                PhoneNumber = "+46-12-3456789",
                Email = "testuser@frozen.se",
                UserName = "testuser@frozen.se"
            };

            return user;
        }

    }
}
