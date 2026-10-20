using BloodDonationPlatform.DTOs;

namespace BloodDonationPlatform.Services.Interfaces;

public interface IBloodRequestService
{
    Task<bool> CreateRequestAsync(BloodRequestDTO requestDto);

    Task<IEnumerable<BloodRequestDTO>> GetPendingRequestsAsync();
}
