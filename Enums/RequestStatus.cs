using BloodDonationPlatform.Enums;

namespace BloodDonationPlatform.Models;

// عقد ثابت: ممنوع تغيير أسماء الخصائص دي من غير اتفاق مع الفريق
public class BloodRequest
{
    public int Id { get; set; }
    public int HospitalId { get; set; }
    public BloodType RequiredBloodType { get; set; }
    public int Quantity { get; set; }
}
