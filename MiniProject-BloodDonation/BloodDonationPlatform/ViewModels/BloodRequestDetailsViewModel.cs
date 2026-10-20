using BloodDonationPlatform.DTOs;

namespace BloodDonationPlatform.ViewModels;

public class BloodRequestDetailsViewModel
{
    public BloodRequestDTO Request { get; set; } = null!;
    public IReadOnlyList<DonorMatchResult> Matches { get; set; } = [];
}
