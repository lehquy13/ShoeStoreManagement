using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ShoeStoreManagement.Models;

namespace ShoeStoreManagement.Services
{
    public partial class GeneralDBContext : DbContext
    {
        public virtual DbSet<Customer> Customers { get; set; } = null!; //v2
        public virtual DbSet<Account> Accounts { get; set; } = null!; // v1
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;//v1

        public virtual DbSet<Product> Products { get; set; } = null!;//v2
        public virtual DbSet<Staff> Staffs { get; set; } = null!;//v2


        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<CartDetail> CartDetails { get; set; } = null!;

        public virtual DbSet<Shipment> Shipments { get; set; } = null!;
        public GeneralDBContext() {}

        public GeneralDBContext(DbContextOptions<GeneralDBContext> options)
            : base(options) {}

       

        
    }
}
