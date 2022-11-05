using Microsoft.EntityFrameworkCore;
using User = ShowerShow.Models.User;
using ShowerShow.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace ShowerShow.DAL
{
    public class DatabaseContext : DbContext
    {

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductReview> ProductReviews { get; set; } = null!;
        
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //secure connection string later
            optionsBuilder.UseCosmos("https://cosmin.documents.azure.com:443/",
               "yEWSz1XH7ys7y44CdOkEaBkuha7tSwXk1TS5XoJKHVOn6qV08J8VpXbeF19YyXBk8WXiTZabILaoCXzPXIXDJw==",
               "cloud-homework");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().ToContainer("Users").HasPartitionKey(c => c.Id);
            modelBuilder.Entity<Order>().ToContainer("Orders").HasPartitionKey(c => c.UserId);
            modelBuilder.Entity<Product>().ToContainer("Products").HasPartitionKey(c => c.Id);
            modelBuilder.Entity<ProductReview>().ToContainer("Reviews").HasPartitionKey(c => c.ProductId);
        }
    }
}
