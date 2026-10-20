using System.ComponentModel.DataAnnotations;
using BloodDonationPlatform.Enums;
using BloodDonationPlatform.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BloodDonationPlatform.ViewModels;

public class CreateBloodRequestViewModel
{
    [Display(Name = "Hospital")]
    public int HospitalId { get; set; }

    [Display(Name = "Required blood type")]
    public BloodType RequiredBloodType { get; set; } = BloodType.OPositive;

    [Range(1, 50)]
    public int Quantity { get; set; } = 1;

    public UrgencyLevel Urgency { get; set; } = UrgencyLevel.Urgent;

    [MaxLength(400)]
    public string? Notes { get; set; }

    [ValidateNever]
    public IReadOnlyList<Hospital> Hospitals { get; set; } = [];
}
