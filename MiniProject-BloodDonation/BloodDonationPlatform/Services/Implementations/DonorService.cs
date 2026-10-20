using BloodDonationPlatform.Data;
using BloodDonationPlatform.Models;
using BloodDonationPlatform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationPlatform.Services.Implementations;

public class DonorService : IDonorService
{
    private readonly ApplicationDbContext _db;

    public DonorService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Donor>> GetAllAsync()
    {
        return await _db.Donors.AsNoTracking().OrderBy(d => d.Name).ToListAsync();
    }

    public Task<Donor?> GetByIdAsync(int id)
    {
        return _db.Donors.FirstOrDefaultAsync(d => d.Id == id);
    }

    public bool IsEligible(Donor donor, DateTime? utcNow = null)
    {
        ArgumentNullException.ThrowIfNull(donor);
        return donor.IsEligibleToDonate(utcNow);
    }

    public async Task MarkDonatedAsync(int donorId, DateTime donatedAtUtc)
    {
        var donor = await _db.Donors.FirstOrDefaultAsync(d => d.Id == donorId);
        if (donor is null)
        {
            return;
        }

        donor.LastDonationDate = donatedAtUtc;
        donor.IsAvailable = false;

        if (!string.IsNullOrEmpty(donor.UserId))
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == donor.UserId);
            if (user is not null)
            {
                user.LastDonationDate = donatedAtUtc;
                user.IsAvailableForDonation = false;
            }
        }

        await _db.SaveChangesAsync();
    }

    public async Task<(bool Success, string Message)> SetAvailabilityAsync(int donorId, bool isAvailable)
    {
        var donor = await _db.Donors.FirstOrDefaultAsync(d => d.Id == donorId);
        if (donor is null)
        {
            return (false, "Donor was not found.");
        }

        donor.IsAvailable = isAvailable;

        if (!string.IsNullOrEmpty(donor.UserId))
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == donor.UserId);
            if (user is not null)
            {
                user.IsAvailableForDonation = isAvailable;
            }
        }

        await _db.SaveChangesAsync();
        return (true, isAvailable ? "Donor is now available." : "Donor marked unavailable.");
    }
}
