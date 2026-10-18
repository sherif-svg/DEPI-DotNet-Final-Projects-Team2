namespace BloodDonationPlatform.Interfaces;

public interface ILocationService
{
    double CalculateDistance(double lat1, double lon1, double lat2, double lon2);
}
