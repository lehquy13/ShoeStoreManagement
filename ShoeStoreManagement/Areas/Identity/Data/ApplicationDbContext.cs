using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Models;

namespace ShoeStoreManagement.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    //public virtual DbSet<Customer> Customers { get; set; } = null!; //v2
    //public virtual DbSet<Account> Accounts { get; set; } = null!; // v1
    public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;//v1

    public virtual DbSet<Product> Products { get; set; } = null!;//v2
    //public virtual DbSet<Staff> Staffs { get; set; } = null!;//v2


    public virtual DbSet<Cart> Carts { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
    public virtual DbSet<CartDetail> CartDetails { get; set; } = null!;

    public virtual DbSet<Shipment> Shipments { get; set; } = null!;
}
