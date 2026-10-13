using BloodDonationPlatform.Models;

namespace BloodDonationPlatform.Services.Interfaces;

public interface IMatchingService
{
    List<string> GetCompatibleDonorBloodTypes(string requiredBloodType);
    IEnumerable<Donor> GetMatchingDonors(string requiredBloodType, double hospitalLat, double hospitalLon);
}
