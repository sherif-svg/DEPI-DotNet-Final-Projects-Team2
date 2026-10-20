using BloodDonationPlatform.Enums;
using BloodDonationPlatform.Models;
using BloodDonationPlatform.ViewModels;

namespace BloodDonationPlatform.Services.Interfaces;

public interface IHospitalInventoryService
{
    Task<Hospital?> GetDefaultHospitalAsync();
    Task<IReadOnlyList<Hospital>> GetHospitalsAsync();
    Task<InventoryDashboardViewModel?> GetDashboardAsync(int hospitalId);
    Task<(bool Success, string Message)> AdjustUnitsAsync(int hospitalId, BloodType bloodType, int delta);
    Task<(bool Success, string Message)> DeductAsync(int hospitalId, BloodType bloodType, int quantity);
}
