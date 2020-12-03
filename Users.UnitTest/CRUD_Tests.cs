using Microsoft.VisualStudio.TestTools.UnitTesting;
using Users.Models;

namespace Users.UnitTest
{
    [TestClass]
    public class CRUD_Tests
    {
        [TestMethod]
        public void CreateUser_RegisterNewUser_ReturnsCreatedUser()
        {
            User user = new User()
            {
                FirstName = "Joel",
                LastName = "Felldin",
                Address = "Alfred Nobels Allé",
                City = "Huddinge",
                Zip = "14152",
                PhoneNumber = "07012312312",
                Email = "joel.felldin@iths.se"
            };

            //Assert.AreEqual(Users.CreateUser(user), user);

        }
        [TestMethod]
        public void CreateUser_RegisterNewUser_DoesNotReturnCreatedUser()
        {

        }
    }
}
