namespace BloodDonationPlatform.Models;

public class Hospital
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
    public ICollection<HospitalInventory> Inventory { get; set; } = new List<HospitalInventory>();
    public ICollection<BloodRequest> BloodRequests { get; set; } = new List<BloodRequest>();
}
