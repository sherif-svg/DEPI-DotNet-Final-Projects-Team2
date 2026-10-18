using BloodDonationPlatform.DTOs;
using BloodDonationPlatform.Models;

namespace BloodDonationPlatform.Interfaces;

public interface IMatchingService
{
    List<Donor> GetMatchingDonors(BloodRequestDTO request);
}
