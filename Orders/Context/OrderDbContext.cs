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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>(entity => {
                entity.Property(x => x.Date).IsRequired();
                entity.Property(x => x.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(x => x.StatusId).IsRequired().HasDefaultValue(1);
            });


            builder.Entity<OrderProduct>(entity => {
                entity.Property(x => x.OrderId).IsRequired();
                entity.Property(x => x.ProductId).IsRequired();
                entity.Property(x => x.Name).IsRequired().HasMaxLength(50);  
            });

            builder.Entity<Status>().HasData(
               new Status() { Id = 1, Name = "Lagd" },
               new Status() { Id = 2, Name = "Behandlad" },
               new Status() { Id = 3, Name = "Skickad" }
              
             
           );

        }

    }
}
