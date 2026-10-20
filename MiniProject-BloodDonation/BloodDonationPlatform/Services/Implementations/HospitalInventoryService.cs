using BloodDonationPlatform.Data;
using BloodDonationPlatform.Enums;
using BloodDonationPlatform.Models;
using BloodDonationPlatform.Services.Interfaces;
using BloodDonationPlatform.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationPlatform.Services.Implementations;

public class HospitalInventoryService : IHospitalInventoryService
{
    private const int GaugeCapacity = 40;
    private readonly ApplicationDbContext _db;

    public HospitalInventoryService(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<Hospital?> GetDefaultHospitalAsync()
    {
        return _db.Hospitals.AsNoTracking().OrderBy(h => h.Id).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<Hospital>> GetHospitalsAsync()
    {
        return await _db.Hospitals.AsNoTracking().OrderBy(h => h.Name).ToListAsync();
    }

    public async Task<InventoryDashboardViewModel?> GetDashboardAsync(int hospitalId)
    {
        var hospital = await _db.Hospitals
            .Include(h => h.Inventory)
            .FirstOrDefaultAsync(h => h.Id == hospitalId);

        if (hospital is null)
        {
            return null;
        }

        EnsureCompleteInventory(hospital);
        await _db.SaveChangesAsync();

        var gauges = Enum.GetValues<BloodType>()
            .Select(type =>
            {
                var units = hospital.Inventory.First(i => i.BloodType == type).Units;
                return new InventoryGaugeViewModel
                {
                    BloodType = type,
                    Units = units,
                    Percent = Math.Clamp((int)Math.Round(units / (double)GaugeCapacity * 100), 0, 100),
                    IsLowStock = units < HospitalInventory.LowStockThreshold
                };
            })
            .ToList();

        return new InventoryDashboardViewModel
        {
            Hospital = hospital,
            Gauges = gauges,
            LowStockCount = gauges.Count(g => g.IsLowStock)
        };
    }

    public Task<(bool Success, string Message)> AdjustUnitsAsync(int hospitalId, BloodType bloodType, int delta)
    {
        return ChangeUnitsAsync(hospitalId, bloodType, current => current + delta, "Inventory updated.");
    }

    public Task<(bool Success, string Message)> DeductAsync(int hospitalId, BloodType bloodType, int quantity)
    {
        if (quantity <= 0)
        {
            return Task.FromResult((false, "Quantity to deduct must be greater than zero."));
        }

        return ChangeUnitsAsync(
            hospitalId,
            bloodType,
            current => current - quantity,
            $"{quantity} unit(s) deducted from {bloodType.ToDisplay()} stock.");
    }

    private async Task<(bool Success, string Message)> ChangeUnitsAsync(
        int hospitalId,
        BloodType bloodType,
        Func<int, int> mutator,
        string successMessage)
    {
        var hospital = await _db.Hospitals
            .Include(h => h.Inventory)
            .FirstOrDefaultAsync(h => h.Id == hospitalId);

        if (hospital is null)
        {
            return (false, "Hospital was not found.");
        }

        EnsureCompleteInventory(hospital);
        var item = hospital.Inventory.First(i => i.BloodType == bloodType);
        var next = mutator(item.Units);
        if (next < 0)
        {
            return (false, $"Cannot reduce {bloodType.ToDisplay()} below zero. Current stock: {item.Units} unit(s).");
        }

        item.Units = next;
        await _db.SaveChangesAsync();
        return (true, successMessage);
    }

    private static void EnsureCompleteInventory(Hospital hospital)
    {
        foreach (var type in Enum.GetValues<BloodType>())
        {
            if (hospital.Inventory.All(i => i.BloodType != type))
            {
                hospital.Inventory.Add(new HospitalInventory
                {
                    HospitalId = hospital.Id,
                    BloodType = type,
                    Units = 0
                });
            }
        }
    }
}
