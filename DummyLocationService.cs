using BloodDonationPlatform.Interfaces;

namespace BloodDonationPlatform.Services;

// تطبيق وهمي مؤقت: بيرجّع رقم عشوائي لحد ما الكود الحقيقي (NetTopologySuite) يخلص
public class DummyLocationService : ILocationService
{
    public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        return Math.Round(Random.Shared.NextDouble() * 50, 2); // من 0 لـ 50 كم
    }
}
