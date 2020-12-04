using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Models;

namespace Users.Context
{
    public class AdminUserRoles
    {
        private readonly UserDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        enum Role { Admin, User }

        public AdminUserRoles(UserDbContext context, UserManager<User> userManager, RoleManager<AppRole> roleManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task Initialize()
        {
            _context.Database.EnsureCreated();

            await CreateRoles();
            await CreateDefaultAdminAccount();
        }

        public async Task CreateRoles()
        {
            var adminRole = Enum.GetName(typeof(Role), Role.Admin);
            if (await _roleManager.FindByNameAsync(adminRole) == null)
                await _roleManager.CreateAsync(new AppRole(adminRole));

            var userRole = Enum.GetName(typeof(Role), Role.User);
            if (await _roleManager.FindByNameAsync(userRole) == null)
                await _roleManager.CreateAsync(new AppRole(userRole));
        }

        public async Task CreateDefaultAdminAccount()
        {
            var adminEmail = "admin@frozen.se";
            var firstName = "IceIce";
            var lastName = "Baby";
            var password = "Test123!";

            var admin = await _userManager.FindByEmailAsync(adminEmail);

            if(admin == null)
            {
                User administrator = new User()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = adminEmail,
                    Email = adminEmail,
                };

                var result = await _userManager.CreateAsync(administrator, password);

                if(result.Succeeded)
                {
                    var role = Enum.GetName(typeof(Role), Role.Admin);
                    await _userManager.AddToRoleAsync(administrator, role);
                }
            }
        }
    }
}
