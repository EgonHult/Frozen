using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Users.Context;
using Users.Models;

namespace Users.UnitTest.Context
{
    public class TestUsersDbContext : IDisposable
    {
        public AppSettings AppsettingsConfig { get; set; }
        public UserDbContext DbContext { get; set; }
        public UserManager<User> UserManager { get; set; }
        public RoleManager<AppRole> RoleManager { get; set; }



        public TestUsersDbContext()
        {
            AppsettingsConfig = new AppSettings();
            DbContext = InitializeContext();
        }

        public UserDbContext InitializeContext()
        {
            var config = AppsettingsConfig.Config.GetConnectionString("SqlDatabase");
            var dbOption = new DbContextOptionsBuilder<UserDbContext>()
                .UseSqlServer(config).Options;
            /*
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<UserDbContext>(options => options.UseSqlServer(config));
            //serviceCollection.AddIdentity<User, AppRole>();

            serviceCollection.AddIdentity<User, AppRole>()
                .AddUserManager<UserManager<User>>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();

            UserManager = serviceCollection.BuildServiceProvider().GetService<UserManager<User>>();
            RoleManager = serviceCollection.BuildServiceProvider().GetService<RoleManager<AppRole>>();
            */
            return new UserDbContext(dbOption);
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
