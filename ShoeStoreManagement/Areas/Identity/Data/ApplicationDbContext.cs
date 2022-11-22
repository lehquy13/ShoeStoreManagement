using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Core.Models;

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
    }
    /// <summary>
    /// Migration ver1
    /// </summary>
    /// 
    public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
    public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
    public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;
    public virtual DbSet<Product> Products { get; set; } = null!;
    public virtual DbSet<Address> Addresses { get; set; } = null!;

    public virtual DbSet<SizeDetail> SizeDetails { get; set; } = null!;

    ///// <summary>
    ///// Migration ver2
    ///// </summary>
    ///// 
    public virtual DbSet<Cart> Carts { get; set; } = null!;
    public virtual DbSet<CartDetail> CartDetails { get; set; } = null!;

    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;

    public virtual DbSet<Voucher> Vouchers { get; set; } = null!;

    ///// <summary>
    ///// Migration ver3
    ///// </summary>
    ///// 
    ///
    public virtual DbSet<WishList> WishLists { get; set; } = null!;
    public virtual DbSet<WishListDetail> WishListDetails { get; set; } = null!;

    public virtual DbSet<Image> Images { get; set; } = null!;

}
