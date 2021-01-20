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

        public DbSet<ProductModel> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProductModel>(entity => {
                entity.Property(x => x.Name).IsRequired();
                entity.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(x => x.Quantity).IsRequired();
                entity.Property(x => x.WeightInGrams).IsRequired();
                entity.Property(x => x.Image).HasDefaultValue("https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.pinterest.com%2Fpin%2F565061084473474980%2F&psig=AOvVaw0qlVouwPj93q6JzQBnUbBU&ust=1611230242855000&source=images&cd=vfe&ved=0CAIQjRxqFwoTCIDd2MS6qu4CFQAAAAAdAAAAABAD");
                entity.Property(x => x.Details).HasDefaultValue("Information om produkten saknas...");
            });
        }
    }
}
