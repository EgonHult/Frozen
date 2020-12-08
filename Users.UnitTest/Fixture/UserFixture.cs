using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Users.Models;

namespace Users.UnitTest.Fixture
{
    public class UserFixture
    {
        private readonly UserManager<User> _userManager;

        public UserFixture(UserManager<User> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<User> CreateDummyUserAsync()
        {
            var dummyUser = DummyUser.TestUser();
            var result = await _userManager.CreateAsync(dummyUser, "Test123!");
            var role = await _userManager.AddToRoleAsync(dummyUser, "User");

            if (result.Succeeded && role.Succeeded)
            {
                return dummyUser;
            }

            return null;
        }

        public async Task RemoveDummyUserAsync(User user)
        {
            await _userManager.DeleteAsync(user);
        }
    }
}
