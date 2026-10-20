using BloodDonationPlatform.DTOs;
using BloodDonationPlatform.Enums;

namespace BloodDonationPlatform.ViewModels;

public class DonorSearchViewModel
{
    public BloodType? BloodType { get; set; }
    public double Latitude { get; set; } = 30.0444;
    public double Longitude { get; set; } = 31.2357;
    public IReadOnlyList<DonorMatchResult> Results { get; set; } = [];
}
