using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QualityBagsECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBagsECommerce.Data
{
    public class BagContext : IdentityDbContext<ApplicationUser>
    {
        public BagContext(DbContextOptions<BagContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Bag> Bags { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Supplier>().ToTable("Supplier");

            modelBuilder.Entity<Bag>().ToTable("Bag");

            modelBuilder.Entity<CartItem>().ToTable("CartItem");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<OrderDetail>().ToTable("OrderDetail");
            modelBuilder.Entity<OrderDetail>().HasOne(p => p.Order).WithMany(o => o.OrderDetails).OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);

        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
    }
}
