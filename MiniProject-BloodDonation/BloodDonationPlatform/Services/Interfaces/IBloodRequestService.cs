using BloodDonationPlatform.DTOs;
using BloodDonationPlatform.Enums;

namespace BloodDonationPlatform.Services.Interfaces;

public interface IBloodRequestService
{
    Task<bool> CreateRequestAsync(BloodRequestDTO requestDto);
    Task<IEnumerable<BloodRequestDTO>> GetPendingRequestsAsync();
    Task<IReadOnlyList<BloodRequestDTO>> GetAllAsync();
    Task<BloodRequestDTO?> GetByIdAsync(int id);
    Task<(bool Success, string Message)> StartMatchingAsync(int requestId);
    Task<(bool Success, string Message)> FulfillAsync(int requestId, int donorId);
    Task<(bool Success, string Message)> CancelAsync(int requestId);
    Task<IReadOnlyList<DonorMatchResult>> GetRankedMatchesAsync(int requestId);
}
