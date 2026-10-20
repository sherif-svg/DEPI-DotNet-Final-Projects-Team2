using BloodDonationPlatform.Models;

namespace BloodDonationPlatform.Services.Interfaces;

public interface IDonorService
{
    Task<IReadOnlyList<Donor>> GetAllAsync();
    Task<Donor?> GetByIdAsync(int id);
    bool IsEligible(Donor donor, DateTime? utcNow = null);
    Task MarkDonatedAsync(int donorId, DateTime donatedAtUtc);
    Task<(bool Success, string Message)> SetAvailabilityAsync(int donorId, bool isAvailable);
}
