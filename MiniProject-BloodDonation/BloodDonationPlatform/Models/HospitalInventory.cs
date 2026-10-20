using BloodDonationPlatform.Enums;

namespace BloodDonationPlatform.Models;

public class HospitalInventory
{
    public const int LowStockThreshold = 5;

    public int Id { get; set; }
    public int HospitalId { get; set; }
    public Hospital Hospital { get; set; } = null!;
    public BloodType BloodType { get; set; }
    public int Units { get; set; }

    public bool IsLowStock => Units < LowStockThreshold;
}
