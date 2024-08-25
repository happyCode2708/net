using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyApi.application.common.interfaces;
using MyApi.Models;

namespace MyApi.data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        // Define your DbSets (tables) here
        // public DbSet<YourEntity> YourEntities { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasKey(p => p.Id);

            modelBuilder.Entity<Image>().HasKey(i => i.Id);

            modelBuilder.Entity<ProductImage>().HasKey(pi => new { pi.ProductId, pi.ImageId });

            modelBuilder.Entity<ProductImage>().HasOne(pi => pi.Product).WithMany(p => p.ProductImages).HasForeignKey(pi => pi.ProductId);

            modelBuilder.Entity<ProductImage>().HasOne(pi => pi.Image).WithMany(i => i.ProductImages).HasForeignKey(pi => pi.ImageId);

        }

    }
}