using System.ComponentModel.DataAnnotations;

namespace BloodDonationPlatform.Enums;

public enum UrgencyLevel
{
    [Display(Name = "Routine")]
    Routine,
    [Display(Name = "Urgent")]
    Urgent,
    [Display(Name = "Critical")]
    Critical
}
