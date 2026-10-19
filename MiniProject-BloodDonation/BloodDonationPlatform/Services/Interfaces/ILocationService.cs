namespace BloodDonationPlatform.Services.Interfaces;

public interface ILocationService
{
    double CalculateDistanceInKilometers(double lat1, double lon1, double lat2, double lon2);
}
