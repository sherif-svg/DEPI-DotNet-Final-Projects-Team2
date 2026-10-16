using BloodDonationPlatform.Models;
using BloodDonationPlatform.Services.Interfaces;

namespace BloodDonationPlatform.Services.Implementations
{
    public class BloodRequestService : IBloodRequestService
    {
        public async Task<bool> CreateRequestAsync(BloodRequestDTO requestDto)
        {
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<BloodRequestDTO>> GetPendingRequestsAsync()
        {
            var mockData = new List<BloodRequestDTO>
            {
                new BloodRequestDTO
                {
                    HospitalId = 1,
                    RequiredBloodType = "A+",
                    Quantity = 5,
                    Status = "Pending"
                }
            };

            return await Task.FromResult(mockData);
        }
    }
}
