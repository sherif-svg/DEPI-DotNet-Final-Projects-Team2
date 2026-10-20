using BloodDonationPlatform.DTOs;
using BloodDonationPlatform.Enums;
using BloodDonationPlatform.Services.Interfaces;
using BloodDonationPlatform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationPlatform.Controllers;

public class DonorsController : Controller
{
    private readonly IDonorService _donorService;
    private readonly IMatchingService _matchingService;
    private readonly ILocationService _locationService;

    public DonorsController(
        IDonorService donorService,
        IMatchingService matchingService,
        ILocationService locationService)
    {
        _donorService = donorService;
        _matchingService = matchingService;
        _locationService = locationService;
    }

    public async Task<IActionResult> Index()
    {
        var donors = await _donorService.GetAllAsync();
        return View(donors);
    }

    public async Task<IActionResult> Search(DonorSearchViewModel model)
    {
        var donors = await _donorService.GetAllAsync();
        if (model.BloodType is BloodType requiredType)
        {
            model.Results = _matchingService.RankMatches(
                requiredType, model.Latitude, model.Longitude, donors);
        }
        else
        {
            model.Results = donors
                .Select(donor =>
                {
                    var distance = _locationService.CalculateDistanceInKilometers(
                        model.Latitude, model.Longitude, donor.Latitude, donor.Longitude);
                    return new DonorMatchResult
                    {
                        Donor = donor,
                        DistanceKm = Math.Round(distance, 1),
                        CompatibilityScore = 0,
                        IsEligible = _donorService.IsEligible(donor),
                        RankScore = 0
                    };
                })
                .OrderByDescending(m => m.IsEligible)
                .ThenBy(m => m.DistanceKm)
                .ToList();
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Donor")]
    public async Task<IActionResult> ToggleAvailability(int id, bool isAvailable, string? returnUrl)
    {
        var result = await _donorService.SetAvailabilityAsync(id, isAvailable);
        TempData[result.Success ? "StatusMessage" : "ErrorMessage"] = result.Message;
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Index));
    }
}
