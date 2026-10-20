using BloodDonationPlatform.Enums;

namespace BloodDonationPlatform.Models;

public class Donor
{
    public const int MinimumDaysBetweenDonations = 56;

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public BloodType BloodType { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime? LastDonationDate { get; set; }
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }

    public bool IsEligibleToDonate(DateTime? utcNow = null)
    {
        var now = utcNow ?? DateTime.UtcNow;
        if (!IsAvailable)
        {
            return false;
        }

        if (LastDonationDate is null)
        {
            return true;
        }

        return (now.Date - LastDonationDate.Value.Date).TotalDays >= MinimumDaysBetweenDonations;
    }

    public int DaysUntilEligible(DateTime? utcNow = null)
    {
        if (LastDonationDate is null)
        {
            return 0;
        }

        var now = utcNow ?? DateTime.UtcNow;
        var remaining = MinimumDaysBetweenDonations - (int)(now.Date - LastDonationDate.Value.Date).TotalDays;
        return Math.Max(0, remaining);
    }
}
