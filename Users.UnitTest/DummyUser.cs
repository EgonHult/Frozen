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
        public static User User()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Arne",
                LastName = "Anka",
                Email = "arne@anka.se"
            };

            return user;
        }
    }
}
