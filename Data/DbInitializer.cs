using Microsoft.AspNetCore.Identity;
using Mini_Project_DEPI.Models;

namespace Mini_Project_DEPI.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // 1. Seed Roles
            string[] roleNames = { "Admin", "User", "Donor", "Hospital" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Seed Default Admin
            string adminEmail = "admin@blooddonation.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Admin",
                    EmailConfirmed = true,
                    IsAvailableForDonation = false
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin@123456");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}