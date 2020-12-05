using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Orders.Models;

namespace Orders.Context
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext()
        {
        }

        public OrderDbContext(DbContextOptions<OrderDbContext> options): base(options)
        {

        }

        public DbSet<Order> Order { get; set; }
        public DbSet<OrderProduct> OrderProduct { get; set; }
        public DbSet<Status> Status { get; set; }

    }
}
