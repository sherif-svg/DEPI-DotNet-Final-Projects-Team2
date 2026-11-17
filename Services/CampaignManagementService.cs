using AutoMapper;
using FluentValidation;
using SocialMediaManager.Application.Common;
using SocialMediaManager.Application.DTOs.Campaigns;
using SocialMediaManager.Application.Interfaces;
using SocialMediaManager.Domain.Entities;
using SocialMediaManager.Domain.Enums;

namespace SocialMediaManager.Application.Services;

public class CampaignManagementService : ICampaignManagementService
{
    private readonly ICampaignRepository _repo;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateCampaignDto> _createValidator;
    private readonly IValidator<UpdateCampaignDto> _updateValidator;

    public CampaignManagementService(
        ICampaignRepository repo,
        IMapper mapper,
        IValidator<CreateCampaignDto> createValidator,
        IValidator<UpdateCampaignDto> updateValidator)
    {
        _repo = repo;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<CampaignDto> CreateAsync(CreateCampaignDto dto)
    {
        await _createValidator.ValidateAndThrowAsync(dto);   // 1) اتأكد من الداتا
        var campaign = _mapper.Map<Campaign>(dto);           // 2) حوّل لـ Entity
        campaign.Status = CampaignStatus.Draft;              // 3) الحملة الجديدة مسودة
        await _repo.AddAsync(campaign);                      // 4) احفظ
        return _mapper.Map<CampaignDto>(campaign);           // 5) رجّع DTO
    }

    public async Task<CampaignDto> UpdateAsync(UpdateCampaignDto dto)
    {
        await _updateValidator.ValidateAndThrowAsync(dto);
        var campaign = await _repo.GetByIdAsync(dto.Id)
            ?? throw new NotFoundException(nameof(Campaign), dto.Id);
        _mapper.Map(dto, campaign);
        await _repo.UpdateAsync(campaign);
        return _mapper.Map<CampaignDto>(campaign);
    }

    public async Task DeleteAsync(int id)
    {
        var campaign = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Campaign), id);
        await _repo.DeleteAsync(campaign);
    }

    public async Task<CampaignDto> GetByIdAsync(int id)
    {
        var campaign = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Campaign), id);
        return _mapper.Map<CampaignDto>(campaign);
    }

    public async Task<List<CampaignDto>> GetAllAsync()
    {
        var campaigns = await _repo.GetAllAsync();
        return _mapper.Map<List<CampaignDto>>(campaigns);
    }
}
