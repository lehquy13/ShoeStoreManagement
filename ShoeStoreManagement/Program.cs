using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ShoeStoreManagement.Data;
using ShoeStoreManagement.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
//builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//add DI for CRUD
builder.Services.AddScoped<IProductCRUD, ProductCRUD>();
builder.Services.AddScoped<IProductCategoryCRUD, ProductCategoryCRUD>();
builder.Services.AddScoped<ISizeDetailCRUD, SizeDetailCRUD>();
builder.Services.AddScoped<IAddressCRUD, AddressCRUD>();
builder.Services.AddScoped<ICartCRUD, CartCRUD>();
builder.Services.AddScoped<ICartDetailCRUD, CartDetailCRUD>();
builder.Services.AddScoped<IOrderCRUD, OrderCRUD>();
builder.Services.AddScoped<IOrderDetailCRUD, OrderDetailCRUD>();
builder.Services.AddScoped<ISupplierCRUD, SupplierCRUD>();


builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Admin",
        pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();
    //app.MapBlazorHub();
});



app.Run();
