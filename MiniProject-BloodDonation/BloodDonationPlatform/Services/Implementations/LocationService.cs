using BloodDonationPlatform.Services.Interfaces;

namespace BloodDonationPlatform.Services.Implementations;

public class LocationService : ILocationService
{
    private const double EarthRadiusKilometers = 6371.0;

    public double CalculateDistanceInKilometers(double lat1, double lon1, double lat2, double lon2)
    {
        var dLat = DegreesToRadians(lat2 - lat1);
        var dLon = DegreesToRadians(lon2 - lon1);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return EarthRadiusKilometers * c;
    }

    private static double DegreesToRadians(double degrees) => degrees * Math.PI / 180.0;
}
