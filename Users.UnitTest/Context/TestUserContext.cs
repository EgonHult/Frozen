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
    public class TestUserContext : IDisposable
    {
        public AppSettings AppsettingsConfig { get; set; }
        public UserDbContext UserDbContext { get; set; }
        public UserManager<User> UserManager { get; set; }
        public SignInManager<User> SignInManager { get; set; }
        public RoleManager<AppRole> RoleManager { get; set; }

        public TestUserContext()
        {
            AppsettingsConfig = new AppSettings();
            InitializeContext();
        }

        public void InitializeContext()
        {
            var config = AppsettingsConfig.Config.GetConnectionString("SqlDatabase");
            var dbOption = new DbContextOptionsBuilder<UserDbContext>()
                .UseSqlServer(config).Options;

            AddIdentityFramework(config);

            UserDbContext = new UserDbContext(dbOption);
        }

        public void AddIdentityFramework(string connectionString)
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            // Add database context and logging or IdentityFramework wont load properly
            serviceCollection.AddDbContext<UserDbContext>(options => options.UseSqlServer(connectionString));
            serviceCollection.AddLogging();

            // Add IdentityFramework and SQL-connection
            serviceCollection.AddIdentity<User, AppRole>()
                .AddUserManager<UserManager<User>>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();

            UserManager = serviceCollection.BuildServiceProvider()
                .GetService<UserManager<User>>();

            SignInManager = serviceCollection.BuildServiceProvider()
                .GetService<SignInManager<User>>();

            RoleManager = serviceCollection.BuildServiceProvider()
                .GetService<RoleManager<AppRole>>();
        }

        public void Dispose()
        {
            UserDbContext?.Dispose();
            UserManager?.Dispose();
            RoleManager?.Dispose();
        }
    }
}
