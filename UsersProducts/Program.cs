using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UsersProducts.Data;
using UsersProducts.Areas.Identity.Data;
using UsersProducts.Services;
using Serilog.Events;
using Serilog;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

var abc = $"{builder.Environment.ContentRootPath}{Path.DirectorySeparatorChar}log.txt";
Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .Enrich.FromLogContext()
        .WriteTo.File($"{builder.Environment.ContentRootPath}{Path.DirectorySeparatorChar}log.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
var connectionString = builder.Configuration.GetConnectionString("UsersProductsContextConnection") ?? throw new InvalidOperationException("Connection string 'UsersProductsContextConnection' not found.");
Log.Logger.Information("Start Logging");
builder.Services.AddDbContext<UsersProductsContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UsersProductsContext>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DbInitializer.Initialize(services);
    }
    catch (Exception ex)
    {
        Log.Logger.Error(ex, "");
    }
}

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

