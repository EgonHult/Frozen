using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Models;

namespace Users.UnitTest
{
    public static class DummyUser
    {
        public static UserModel TestUser()
        {
            UserModel  user = new UserModel()
            {
                FirstName = "Joel",
                LastName = "Felldin",
                Address = "Alfred Nobels Allé",
                City = "Huddinge",
                Zip = "14152",
                PhoneNumber = "07012312312",
                Email = "joel.felldin@iths.se"
            };

            return user;
        }

        public static User TestUser2()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                Email = "test@test.com"
            };

            return user;
        }

    }
}
