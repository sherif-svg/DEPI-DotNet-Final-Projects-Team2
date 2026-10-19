using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Mini_Project_DEPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(5)]
        public string? BloodType { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        public bool IsAvailableForDonation { get; set; } = true;

        public DateTime? LastDonationDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}