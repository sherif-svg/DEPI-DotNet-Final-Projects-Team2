using BloodDonationPlatform.Data;
using BloodDonationPlatform.DTOs;
using BloodDonationPlatform.Enums;
using BloodDonationPlatform.Models;
using BloodDonationPlatform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationPlatform.Services.Implementations;

public class BloodRequestService : IBloodRequestService
{
    private readonly ApplicationDbContext _db;
    private readonly IMatchingService _matchingService;
    private readonly IDonorService _donorService;
    private readonly IHospitalInventoryService _inventoryService;

    public BloodRequestService(
        ApplicationDbContext db,
        IMatchingService matchingService,
        IDonorService donorService,
        IHospitalInventoryService inventoryService)
    {
        _db = db;
        _matchingService = matchingService;
        _donorService = donorService;
        _inventoryService = inventoryService;
    }

    public async Task<bool> CreateRequestAsync(BloodRequestDTO requestDto)
    {
        ArgumentNullException.ThrowIfNull(requestDto);

        if (requestDto.Quantity <= 0)
        {
            return false;
        }

        var hospitalExists = await _db.Hospitals.AnyAsync(h => h.Id == requestDto.HospitalId);
        if (!hospitalExists)
        {
            return false;
        }

        _db.BloodRequests.Add(new BloodRequest
        {
            HospitalId = requestDto.HospitalId,
            RequiredBloodType = requestDto.RequiredBloodType,
            Quantity = requestDto.Quantity,
            Urgency = requestDto.Urgency,
            Notes = requestDto.Notes,
            Status = RequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<BloodRequestDTO>> GetPendingRequestsAsync()
    {
        var all = await GetAllAsync();
        return all.Where(r => r.Status is RequestStatus.Pending or RequestStatus.Matching);
    }

    public async Task<IReadOnlyList<BloodRequestDTO>> GetAllAsync()
    {
        var requests = await _db.BloodRequests
            .AsNoTracking()
            .Include(r => r.Hospital)
            .Include(r => r.MatchedDonor)
            .OrderByDescending(r => r.Urgency)
            .ThenByDescending(r => r.CreatedAt)
            .ToListAsync();

        return requests.Select(Map).ToList();
    }

    public async Task<BloodRequestDTO?> GetByIdAsync(int id)
    {
        var request = await _db.BloodRequests
            .AsNoTracking()
            .Include(r => r.Hospital)
            .Include(r => r.MatchedDonor)
            .FirstOrDefaultAsync(r => r.Id == id);

        return request is null ? null : Map(request);
    }

    public async Task<(bool Success, string Message)> StartMatchingAsync(int requestId)
    {
        var request = await _db.BloodRequests.FirstOrDefaultAsync(r => r.Id == requestId);
        if (request is null)
        {
            return (false, "Request was not found.");
        }

        if (request.Status is RequestStatus.Fulfilled or RequestStatus.Cancelled)
        {
            return (false, "Closed requests cannot enter matching.");
        }

        request.Status = RequestStatus.Matching;
        await _db.SaveChangesAsync();
        return (true, "Matching pipeline started.");
    }

    public async Task<(bool Success, string Message)> FulfillAsync(int requestId, int donorId)
    {
        var request = await _db.BloodRequests
            .Include(r => r.Hospital)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request is null)
        {
            return (false, "Request was not found.");
        }

        if (request.Status is RequestStatus.Fulfilled or RequestStatus.Cancelled)
        {
            return (false, "This request is already closed.");
        }

        var donor = await _donorService.GetByIdAsync(donorId);
        if (donor is null)
        {
            return (false, "Donor was not found.");
        }

        if (!_donorService.IsEligible(donor))
        {
            return (false, "Donor is not eligible (56-day gap or availability).");
        }

        var compatible = _matchingService.GetCompatibleDonorBloodTypes(request.RequiredBloodType);
        if (!compatible.Contains(donor.BloodType))
        {
            return (false, "Donor blood type is not ABO/Rh compatible.");
        }

        var deduct = await _inventoryService.DeductAsync(request.HospitalId, request.RequiredBloodType, request.Quantity);
        if (!deduct.Success)
        {
            return deduct;
        }

        var now = DateTime.UtcNow;
        await _donorService.MarkDonatedAsync(donor.Id, now);

        request.MatchedDonorId = donor.Id;
        request.Status = RequestStatus.Fulfilled;
        request.FulfilledAt = now;
        await _db.SaveChangesAsync();

        return (true, "Request fulfilled. Inventory deducted and donor last-donation date updated.");
    }

    public async Task<(bool Success, string Message)> CancelAsync(int requestId)
    {
        var request = await _db.BloodRequests.FirstOrDefaultAsync(r => r.Id == requestId);
        if (request is null)
        {
            return (false, "Request was not found.");
        }

        if (request.Status == RequestStatus.Fulfilled)
        {
            return (false, "Fulfilled requests cannot be cancelled.");
        }

        request.Status = RequestStatus.Cancelled;
        await _db.SaveChangesAsync();
        return (true, "Request cancelled.");
    }

    public async Task<IReadOnlyList<DonorMatchResult>> GetRankedMatchesAsync(int requestId)
    {
        var request = await _db.BloodRequests
            .Include(r => r.Hospital)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request is null)
        {
            return [];
        }

        var donors = await _donorService.GetAllAsync();
        return _matchingService.RankMatches(
            request.RequiredBloodType,
            request.Hospital.Latitude,
            request.Hospital.Longitude,
            donors);
    }

    private static BloodRequestDTO Map(BloodRequest request) => new()
    {
        Id = request.Id,
        HospitalId = request.HospitalId,
        HospitalName = request.Hospital?.Name ?? $"Hospital #{request.HospitalId}",
        RequiredBloodType = request.RequiredBloodType,
        Quantity = request.Quantity,
        Status = request.Status,
        Urgency = request.Urgency,
        MatchedDonorId = request.MatchedDonorId,
        MatchedDonorName = request.MatchedDonor?.Name,
        Notes = request.Notes,
        CreatedAt = request.CreatedAt
    };
}
