using BloodDonationPlatform.Enums;
using BloodDonationPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationPlatform.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await db.Database.MigrateAsync();

        string[] roleNames = ["Admin", "User", "Donor", "Hospital"];
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        await EnsureUserAsync(
            userManager,
            "admin@blooddonation.com",
            "Admin@123456",
            "System Admin",
            "Admin",
            bloodType: null,
            available: false);

        var hospitalUser = await EnsureUserAsync(
            userManager,
            "cairo.hospital@health.gov.eg",
            "Hosp@123456",
            "Cairo University Hospital",
            "Hospital",
            bloodType: null,
            available: false);

        var donorUser = await EnsureUserAsync(
            userManager,
            "donor1@gmail.com",
            "Donor@123456",
            "Ahmed Hassan",
            "Donor",
            bloodType: "O-",
            available: true);

        if (!await db.Hospitals.AnyAsync())
        {
            db.Hospitals.Add(new Hospital
            {
                Name = "Cairo University Hospital",
                City = "Cairo",
                Latitude = 30.0444,
                Longitude = 31.2357,
                UserId = hospitalUser?.Id,
                Inventory =
                [
                    new() { BloodType = BloodType.APositive, Units = 12 },
                    new() { BloodType = BloodType.ANegative, Units = 4 },
                    new() { BloodType = BloodType.BPositive, Units = 10 },
                    new() { BloodType = BloodType.BNegative, Units = 8 },
                    new() { BloodType = BloodType.ABPositive, Units = 6 },
                    new() { BloodType = BloodType.ABNegative, Units = 2 },
                    new() { BloodType = BloodType.OPositive, Units = 18 },
                    new() { BloodType = BloodType.ONegative, Units = 3 }
                ]
            });
            await db.SaveChangesAsync();
        }

        if (!await db.Donors.AnyAsync())
        {
            db.Donors.AddRange(
                new Donor
                {
                    Name = "Ahmed Hassan",
                    BloodType = BloodType.ONegative,
                    Latitude = 30.0444,
                    Longitude = 31.2357,
                    IsAvailable = true,
                    LastDonationDate = DateTime.UtcNow.AddDays(-90),
                    UserId = donorUser?.Id
                },
                new Donor
                {
                    Name = "Sara Ali",
                    BloodType = BloodType.APositive,
                    Latitude = 30.0131,
                    Longitude = 31.2089,
                    IsAvailable = true,
                    LastDonationDate = DateTime.UtcNow.AddDays(-70)
                },
                new Donor
                {
                    Name = "Omar Nabil",
                    BloodType = BloodType.BPositive,
                    Latitude = 31.2001,
                    Longitude = 29.9187,
                    IsAvailable = true,
                    LastDonationDate = DateTime.UtcNow.AddDays(-12)
                },
                new Donor
                {
                    Name = "Layla Mahmoud",
                    BloodType = BloodType.ABNegative,
                    Latitude = 30.0626,
                    Longitude = 31.2497,
                    IsAvailable = false,
                    LastDonationDate = DateTime.UtcNow.AddDays(-40)
                });
            await db.SaveChangesAsync();
        }

        if (!await db.BloodRequests.AnyAsync())
        {
            var hospitalId = await db.Hospitals.Select(h => h.Id).FirstAsync();
            db.BloodRequests.AddRange(
                new BloodRequest
                {
                    HospitalId = hospitalId,
                    RequiredBloodType = BloodType.APositive,
                    Quantity = 4,
                    Urgency = UrgencyLevel.Urgent,
                    Status = RequestStatus.Pending,
                    Notes = "Surgical backup for tonight.",
                    CreatedAt = DateTime.UtcNow.AddHours(-6)
                },
                new BloodRequest
                {
                    HospitalId = hospitalId,
                    RequiredBloodType = BloodType.ONegative,
                    Quantity = 2,
                    Urgency = UrgencyLevel.Critical,
                    Status = RequestStatus.Matching,
                    Notes = "Trauma bay — universal donor preferred.",
                    CreatedAt = DateTime.UtcNow.AddHours(-2)
                },
                new BloodRequest
                {
                    HospitalId = hospitalId,
                    RequiredBloodType = BloodType.BPositive,
                    Quantity = 3,
                    Urgency = UrgencyLevel.Routine,
                    Status = RequestStatus.Pending,
                    Notes = "Ward restock.",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                });
            await db.SaveChangesAsync();
        }
    }

    private static async Task<ApplicationUser?> EnsureUserAsync(
        UserManager<ApplicationUser> userManager,
        string email,
        string password,
        string fullName,
        string role,
        string? bloodType,
        bool available)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true,
                BloodType = bloodType,
                IsAvailableForDonation = available
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return null;
            }
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            await userManager.AddToRoleAsync(user, role);
        }

        return user;
    }
}
