using Microsoft.AspNetCore.Identity;
using Mutterboard.Models;
using Mutternboard.Models;
using System.Threading.Tasks;

public static class SeedData
{
    public static async Task Initialize(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        string adminEmail = "lukasplol12@gmail.com";
        string userEmail = "test@gmail.com";
        string password = "Plol123Plol123!";

        // Admin-Rolle erstellen
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new ApplicationRole("Admin"));
        }

        // Benutzer-Rolle erstellen
        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new ApplicationRole("User"));
        }

        // Normalen Benutzer erstellen
        var user = await userManager.FindByEmailAsync(userEmail);
        if (user == null)
        {
            user = new ApplicationUser { UserName = userEmail, Email = userEmail };
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
            }
        }
        // Admin-Benutzer erstellen
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
            var result = await userManager.CreateAsync(admin, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
