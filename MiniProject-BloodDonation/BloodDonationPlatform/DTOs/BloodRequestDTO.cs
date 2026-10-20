using BloodDonationPlatform.Enums;

namespace BloodDonationPlatform.DTOs;

public class BloodRequestDTO
{
    public int Id { get; set; }
    public int HospitalId { get; set; }
    public string HospitalName { get; set; } = string.Empty;
    public BloodType RequiredBloodType { get; set; }
    public int Quantity { get; set; }
    public RequestStatus Status { get; set; }
    public UrgencyLevel Urgency { get; set; }
    public int? MatchedDonorId { get; set; }
    public string? MatchedDonorName { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
