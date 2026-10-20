using BloodDonationPlatform.DTOs;
using BloodDonationPlatform.Enums;
using BloodDonationPlatform.Services.Interfaces;

namespace BloodDonationPlatform.Services.Implementations;

public class BloodRequestService : IBloodRequestService
{
    public Task<bool> CreateRequestAsync(BloodRequestDTO requestDto)
    {
        ArgumentNullException.ThrowIfNull(requestDto);

        return Task.FromResult(true);
    }

    public Task<IEnumerable<BloodRequestDTO>> GetPendingRequestsAsync()
    {
        var mockData = new List<BloodRequestDTO>
        {
            new()
            {
                HospitalId = 1,
                RequiredBloodType = BloodType.APositive,
                Quantity = 5,
                Status = RequestStatus.Pending
            }
        };

        return Task.FromResult<IEnumerable<BloodRequestDTO>>(mockData);
    }
}
