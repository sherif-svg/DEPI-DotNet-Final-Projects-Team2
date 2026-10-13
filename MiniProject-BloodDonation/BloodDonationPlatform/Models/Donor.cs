namespace BloodDonationPlatform.Models;

public class Donor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BloodType { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
