using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Orders.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.UnitTest.Context
{
   public class TestOrdersDbContext : IDisposable
    {
        public AppSettings AppsettingsConfig { get; set; }
        public OrderDbContext DbContext { get; set; }

        public TestOrdersDbContext()
        {
            AppsettingsConfig = new AppSettings();
            DbContext = InitializeContext();
        }

        public OrderDbContext InitializeContext()
        {
            var config = AppsettingsConfig.Config.GetConnectionString("SqlDatabase");
            var dbOption = new DbContextOptionsBuilder<OrderDbContext>()
                .UseSqlServer(config).Options;

            return new OrderDbContext(dbOption);
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
