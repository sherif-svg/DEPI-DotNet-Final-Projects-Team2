using BloodDonationPlatform.Enums;
using BloodDonationPlatform.Services.Interfaces;
using BloodDonationPlatform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationPlatform.Controllers;

public class HospitalsController : Controller
{
    private readonly IHospitalInventoryService _inventoryService;

    public HospitalsController(IHospitalInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public async Task<IActionResult> Inventory()
    {
        var hospital = await _inventoryService.GetDefaultHospitalAsync();
        if (hospital is null)
        {
            return View((InventoryDashboardViewModel?)null);
        }

        var dashboard = await _inventoryService.GetDashboardAsync(hospital.Id);
        return View(dashboard);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Hospital")]
    public async Task<IActionResult> Adjust(int hospitalId, BloodType bloodType, int delta)
    {
        var result = await _inventoryService.AdjustUnitsAsync(hospitalId, bloodType, delta);
        TempData[result.Success ? "StatusMessage" : "ErrorMessage"] = result.Message;
        return RedirectToAction(nameof(Inventory));
    }
}
