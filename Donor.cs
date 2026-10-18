using BloodDonationPlatform.Enums;

namespace BloodDonationPlatform.Models;

// عقد ثابت: ممنوع تغيير أسماء الخصائص دي من غير اتفاق مع الفريق
public class Donor
{
    public int Id { get; set; }
    public BloodType BloodType { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
