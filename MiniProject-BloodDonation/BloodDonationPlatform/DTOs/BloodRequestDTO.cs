using BloodDonationPlatform.Enums;
namespace BloodDonationPlatform.DTOs;
public class BloodRequestDTO
{
    public int Id { get; set; }
    public int HospitalId { get; set; }
    public BloodType RequiredBloodType { get; set; }
    public int Quantity { get; set; }
    public RequestStatus Status { get; set; }
}
