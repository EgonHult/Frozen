using Microsoft.EntityFrameworkCore;
using Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Context
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext()
        {
        }

        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
        {
        }

        public DbSet<ProductsModel> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProductsModel>(entity => {
                entity.Property(x => x.Name).IsRequired();
                entity.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(x => x.Quantity).IsRequired();
                entity.Property(x => x.Image).HasDefaultValue("https://i.pinimg.com/originals/21/ff/a1/21ffa154e3d8639299017ab5683e55cc.jpg");
                entity.Property(x => x.Details).HasDefaultValue("Information om produkten saknas...");
            });
        }
    }
}
