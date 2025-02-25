using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mutterboard.Models;
using Mutternboard.Models;

var builder = WebApplication.CreateBuilder(args);

// Verbindung zur MariaDB herstellen
builder.Services.AddDbContext<Context>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(10, 5, 0))));

// Identity einrichten mit eigener `ApplicationRole`
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
})
    .AddDefaultUI()
.AddEntityFrameworkStores<Context>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// User- und Rolleninitialisierung
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
    await SeedData.Initialize(userManager, roleManager);
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Hier Razor Pages registrieren

app.Run();