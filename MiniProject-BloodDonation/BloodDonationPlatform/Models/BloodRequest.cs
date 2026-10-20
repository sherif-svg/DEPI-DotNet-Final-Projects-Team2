using BloodDonationPlatform.Enums;

namespace BloodDonationPlatform.Models;

public class BloodRequest
{
    public int Id { get; set; }
    public int HospitalId { get; set; }
    public Hospital Hospital { get; set; } = null!;
    public BloodType RequiredBloodType { get; set; }
    public int Quantity { get; set; }
    public RequestStatus Status { get; set; } = RequestStatus.Pending;
    public UrgencyLevel Urgency { get; set; } = UrgencyLevel.Routine;
    public int? MatchedDonorId { get; set; }
    public Donor? MatchedDonor { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? FulfilledAt { get; set; }
}
