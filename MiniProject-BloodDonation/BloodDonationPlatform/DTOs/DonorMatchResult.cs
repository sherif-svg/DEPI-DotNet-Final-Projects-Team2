using BloodDonationPlatform.Models;

namespace BloodDonationPlatform.DTOs;

public class DonorMatchResult
{
    public Donor Donor { get; set; } = null!;
    public double DistanceKm { get; set; }
    public int CompatibilityScore { get; set; }
    public bool IsEligible { get; set; }
    public int RankScore { get; set; }
}
