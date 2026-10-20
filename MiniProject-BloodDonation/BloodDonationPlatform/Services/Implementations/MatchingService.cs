using BloodDonationPlatform.Data;
using BloodDonationPlatform.DTOs;
using BloodDonationPlatform.Enums;
using BloodDonationPlatform.Models;
using BloodDonationPlatform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationPlatform.Services.Implementations;

public class MatchingService : IMatchingService
{
    private readonly ApplicationDbContext _db;
    private readonly ILocationService _locationService;
    private readonly IDonorService _donorService;

    private static readonly Dictionary<BloodType, BloodType[]> CompatibilityMap = new()
    {
        [BloodType.ONegative] = [BloodType.ONegative],
        [BloodType.OPositive] = [BloodType.ONegative, BloodType.OPositive],
        [BloodType.ANegative] = [BloodType.ONegative, BloodType.ANegative],
        [BloodType.APositive] = [BloodType.ONegative, BloodType.OPositive, BloodType.ANegative, BloodType.APositive],
        [BloodType.BNegative] = [BloodType.ONegative, BloodType.BNegative],
        [BloodType.BPositive] = [BloodType.ONegative, BloodType.OPositive, BloodType.BNegative, BloodType.BPositive],
        [BloodType.ABNegative] = [BloodType.ONegative, BloodType.ANegative, BloodType.BNegative, BloodType.ABNegative],
        [BloodType.ABPositive] =
        [
            BloodType.ONegative, BloodType.OPositive, BloodType.ANegative, BloodType.APositive,
            BloodType.BNegative, BloodType.BPositive, BloodType.ABNegative, BloodType.ABPositive
        ]
    };

    public MatchingService(
        ApplicationDbContext db,
        ILocationService locationService,
        IDonorService donorService)
    {
        _db = db;
        _locationService = locationService;
        _donorService = donorService;
    }

    public List<BloodType> GetCompatibleDonorBloodTypes(BloodType requiredBloodType)
    {
        return CompatibilityMap.TryGetValue(requiredBloodType, out var types)
            ? types.ToList()
            : [];
    }

    public List<string> GetCompatibleDonorBloodTypes(string requiredBloodType)
    {
        if (!BloodTypeExtensions.TryParseDisplay(requiredBloodType, out var type))
        {
            return [];
        }

        return GetCompatibleDonorBloodTypes(type).Select(t => t.ToDisplay()).ToList();
    }

    public IEnumerable<Donor> GetMatchingDonors(string requiredBloodType, double hospitalLat, double hospitalLon)
    {
        if (!BloodTypeExtensions.TryParseDisplay(requiredBloodType, out var type))
        {
            return [];
        }

        var donors = _db.Donors.AsNoTracking().ToList();
        return RankMatches(type, hospitalLat, hospitalLon, donors).Select(m => m.Donor);
    }

    public IReadOnlyList<DonorMatchResult> RankMatches(
        BloodType requiredBloodType,
        double hospitalLat,
        double hospitalLon,
        IEnumerable<Donor> donors)
    {
        var compatible = GetCompatibleDonorBloodTypes(requiredBloodType);
        var now = DateTime.UtcNow;

        return donors
            .Where(d => compatible.Contains(d.BloodType))
            .Select(donor =>
            {
                var distance = _locationService.CalculateDistanceInKilometers(
                    hospitalLat, hospitalLon, donor.Latitude, donor.Longitude);
                var eligible = _donorService.IsEligible(donor, now);
                var compatibility = ScoreCompatibility(donor.BloodType, requiredBloodType);
                var rank = (compatibility * 10)
                           - (int)Math.Round(distance)
                           + (eligible ? 50 : -500);

                return new DonorMatchResult
                {
                    Donor = donor,
                    DistanceKm = Math.Round(distance, 1),
                    CompatibilityScore = compatibility,
                    IsEligible = eligible,
                    RankScore = rank
                };
            })
            .OrderByDescending(m => m.IsEligible)
            .ThenByDescending(m => m.CompatibilityScore)
            .ThenBy(m => m.DistanceKm)
            .ToList();
    }

    private static int ScoreCompatibility(BloodType donorType, BloodType requiredType)
    {
        if (donorType == requiredType)
        {
            return 100;
        }

        if (donorType == BloodType.ONegative)
        {
            return 90;
        }

        if (requiredType == BloodType.ABPositive)
        {
            return 80;
        }

        return 70;
    }
}
