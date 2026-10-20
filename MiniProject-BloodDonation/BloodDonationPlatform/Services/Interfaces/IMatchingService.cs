using BloodDonationPlatform.DTOs;
using BloodDonationPlatform.Enums;
using BloodDonationPlatform.Models;

namespace BloodDonationPlatform.Services.Interfaces;

public interface IMatchingService
{
    List<string> GetCompatibleDonorBloodTypes(string requiredBloodType);
    List<BloodType> GetCompatibleDonorBloodTypes(BloodType requiredBloodType);
    IEnumerable<Donor> GetMatchingDonors(string requiredBloodType, double hospitalLat, double hospitalLon);
    IReadOnlyList<DonorMatchResult> RankMatches(
        BloodType requiredBloodType,
        double hospitalLat,
        double hospitalLon,
        IEnumerable<Donor> donors);
}
