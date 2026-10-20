using BloodDonationPlatform.DTOs;
using BloodDonationPlatform.Services.Interfaces;
using BloodDonationPlatform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationPlatform.Controllers;

public class BloodRequestsController : Controller
{
    private readonly IBloodRequestService _bloodRequestService;
    private readonly IHospitalInventoryService _inventoryService;

    public BloodRequestsController(
        IBloodRequestService bloodRequestService,
        IHospitalInventoryService inventoryService)
    {
        _bloodRequestService = bloodRequestService;
        _inventoryService = inventoryService;
    }

    public async Task<IActionResult> Index()
    {
        var requests = await _bloodRequestService.GetAllAsync();
        return View(requests);
    }

    public async Task<IActionResult> Details(int id)
    {
        var request = await _bloodRequestService.GetByIdAsync(id);
        if (request is null)
        {
            return NotFound();
        }

        var matches = await _bloodRequestService.GetRankedMatchesAsync(id);
        return View(new BloodRequestDetailsViewModel
        {
            Request = request,
            Matches = matches
        });
    }

    [Authorize(Roles = "Admin,Hospital")]
    public async Task<IActionResult> Create()
    {
        return View(await BuildCreateModelAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Hospital")]
    public async Task<IActionResult> Create(CreateBloodRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Hospitals = await _inventoryService.GetHospitalsAsync();
            return View(model);
        }

        var created = await _bloodRequestService.CreateRequestAsync(new BloodRequestDTO
        {
            HospitalId = model.HospitalId,
            RequiredBloodType = model.RequiredBloodType,
            Quantity = model.Quantity,
            Urgency = model.Urgency,
            Notes = model.Notes
        });

        if (!created)
        {
            ModelState.AddModelError(string.Empty, "The request could not be created. Check hospital and quantity.");
            model.Hospitals = await _inventoryService.GetHospitalsAsync();
            return View(model);
        }

        TempData["StatusMessage"] = "Blood request created with Pending status.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Hospital")]
    public async Task<IActionResult> StartMatching(int id)
    {
        var result = await _bloodRequestService.StartMatchingAsync(id);
        TempData[result.Success ? "StatusMessage" : "ErrorMessage"] = result.Message;
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Hospital")]
    public async Task<IActionResult> Fulfill(int id, int donorId)
    {
        var result = await _bloodRequestService.FulfillAsync(id, donorId);
        TempData[result.Success ? "StatusMessage" : "ErrorMessage"] = result.Message;
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Hospital")]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _bloodRequestService.CancelAsync(id);
        TempData[result.Success ? "StatusMessage" : "ErrorMessage"] = result.Message;
        return RedirectToAction(nameof(Details), new { id });
    }

    private async Task<CreateBloodRequestViewModel> BuildCreateModelAsync()
    {
        var hospitals = await _inventoryService.GetHospitalsAsync();
        var selected = hospitals.FirstOrDefault()?.Id ?? 0;
        return new CreateBloodRequestViewModel
        {
            HospitalId = selected,
            Hospitals = hospitals
        };
    }
}
