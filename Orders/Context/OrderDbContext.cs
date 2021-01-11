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

        public DbSet<OrderModel> Order { get; set; }
        public DbSet<OrderProductModel> OrderProduct { get; set; }
        public DbSet<StatusModel> Status { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderModel>(entity => {
                entity.Property(x => x.Date).IsRequired();
                entity.Property(x => x.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(x => x.StatusId).IsRequired().HasDefaultValue(1);
            });


            builder.Entity<OrderProductModel>(entity => {
                entity.Property(x => x.OrderModelId).IsRequired();
                entity.Property(x => x.ProductId).IsRequired();
                entity.Property(x => x.Name).IsRequired().HasMaxLength(50);  
            });

            builder.Entity<StatusModel>().HasData(
               new StatusModel() { Id = 1, Name = "Lagd" },
               new StatusModel() { Id = 2, Name = "Behandlad" },
               new StatusModel() { Id = 3, Name = "Skickad" }
              
             
           );

        }

    }
}
