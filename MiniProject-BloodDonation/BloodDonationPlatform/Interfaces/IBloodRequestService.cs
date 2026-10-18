using BloodDonationPlatform.DTOs;
using BloodDonationPlatform.Enums;

namespace BloodDonationPlatform.Interfaces;

public interface IBloodRequestService
{
    List<BloodRequestDTO> GetPendingRequests();
    BloodRequestDTO CreateRequest(BloodRequestDTO request);
    void UpdateRequestStatus(int requestId, RequestStatus newStatus);
}
