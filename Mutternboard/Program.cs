using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mutterboard.Models;

var builder = WebApplication.CreateBuilder(args);

// In-Memory Datenbank anstelle von SQL Server
builder.Services.AddDbContext<Context>(options =>
    options.UseInMemoryDatabase("Tasks"));

// Identity einrichten (keine Best�tigungsmail erforderlich)
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true; // (optional, um doppelte E-Mails zu verhindern)
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<Context>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Muss VOR `builder.Build()` stehen!

var app = builder.Build();

// User- und Rolleninitialisierung
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
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
