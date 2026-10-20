namespace BloodDonationPlatform.Models
{
    public class BloodRequestDTO
    {
        public int HospitalId { get; set; }
        public string RequiredBloodType { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Status { get; set; } = "Pending";
    }
}