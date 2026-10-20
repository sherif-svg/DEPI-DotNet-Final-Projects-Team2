using BloodDonationPlatform.Enums;
using BloodDonationPlatform.Models;

namespace BloodDonationPlatform.ViewModels;

public class InventoryDashboardViewModel
{
    public Hospital Hospital { get; set; } = null!;
    public IReadOnlyList<InventoryGaugeViewModel> Gauges { get; set; } = [];
    public int LowStockCount { get; set; }
}

public class InventoryGaugeViewModel
{
    public BloodType BloodType { get; set; }
    public int Units { get; set; }
    public int Percent { get; set; }
    public bool IsLowStock { get; set; }
}
