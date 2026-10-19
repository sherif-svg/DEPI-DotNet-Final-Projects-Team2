using BloodDonationPlatform.Models;
using BloodDonationPlatform.Services.Interfaces;

namespace BloodDonationPlatform.Services.Implementations;

public class MockMatchingService : IMatchingService
{
    private readonly ILocationService _locationService;

    public MockMatchingService(ILocationService locationService)
    {
        _locationService = locationService;
    }

    // Maps a recipient/required blood type to the donor types that are medically compatible.
    private readonly Dictionary<string, List<string>> _compatibilityMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["O-"] = ["O-"],
        ["O+"] = ["O-", "O+"],
        ["A-"] = ["O-", "A-"],
        ["A+"] = ["O-", "O+", "A-", "A+"],
        ["B-"] = ["O-", "B-"],
        ["B+"] = ["O-", "O+", "B-", "B+"],
        ["AB-"] = ["O-", "A-", "B-", "AB-"],
        ["AB+"] = ["O-", "O+", "A-", "A+", "B-", "B+", "AB-", "AB+"]
    };

    public List<string> GetCompatibleDonorBloodTypes(string requiredBloodType)
    {
        if (string.IsNullOrWhiteSpace(requiredBloodType) ||
            !_compatibilityMap.TryGetValue(requiredBloodType.Trim(), out var compatibleTypes))
        {
            return [];
        }

        return compatibleTypes;
    }

    public IEnumerable<Donor> GetMatchingDonors(string requiredBloodType, double hospitalLat, double hospitalLon)
    {
        var compatibleTypes = GetCompatibleDonorBloodTypes(requiredBloodType);
        if (compatibleTypes.Count == 0)
        {
            return [];
        }

        var mockDonors = new List<Donor>
        {
            new() { Id = 1, Name = "Ahmed Hassan", BloodType = "O-", Latitude = 30.0444, Longitude = 31.2357 },
            new() { Id = 2, Name = "Sara Ali", BloodType = "A+", Latitude = 30.0131, Longitude = 31.2089 },
            new() { Id = 3, Name = "Omar Nabil", BloodType = "B+", Latitude = 31.2001, Longitude = 29.9187 },
            new() { Id = 4, Name = "Layla Mahmoud", BloodType = "AB-", Latitude = 30.0626, Longitude = 31.2497 }
        };

        return mockDonors
            .Where(d => compatibleTypes.Contains(d.BloodType, StringComparer.OrdinalIgnoreCase))
            .OrderBy(d => _locationService.CalculateDistanceInKilometers(
                hospitalLat, hospitalLon, d.Latitude, d.Longitude));
    }
}
