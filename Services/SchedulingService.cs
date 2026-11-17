using AutoMapper;
using FluentValidation;
using SocialMediaManager.Application.Common;
using SocialMediaManager.Application.DTOs.Posts;
using SocialMediaManager.Application.Interfaces;
using SocialMediaManager.Domain.Entities;

namespace SocialMediaManager.Application.Services;

public class SchedulingService : ISchedulingService
{
    private readonly IPostRepository _posts;
    private readonly ICampaignRepository _campaigns;
    private readonly IPostPublishingService _publisher;
    private readonly IJobScheduler _jobs;
    private readonly IMapper _mapper;
    private readonly IValidator<SchedulePostDto> _validator;

    public SchedulingService(
        IPostRepository posts,
        ICampaignRepository campaigns,
        IPostPublishingService publisher,
        IJobScheduler jobs,
        IMapper mapper,
        IValidator<SchedulePostDto> validator)
    {
        _posts = posts;
        _campaigns = campaigns;
        _publisher = publisher;
        _jobs = jobs;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<PostDto> SchedulePostAsync(SchedulePostDto dto)
    {
        await _validator.ValidateAndThrowAsync(dto);

        var campaign = await _campaigns.GetByIdAsync(dto.CampaignId)
            ?? throw new NotFoundException(nameof(Campaign), dto.CampaignId);

        var post = _mapper.Map<Post>(dto);
        await _posts.AddAsync(post);

        // قول لـ Hangfire ينشر البوست في وقته
        _jobs.SchedulePublish(post.Id, dto.ScheduledAt);

        return _mapper.Map<PostDto>(post);
    }

    public async Task<List<PostDto>> GetCampaignPostsAsync(int campaignId)
    {
        var posts = await _posts.GetByCampaignIdAsync(campaignId);
        return _mapper.Map<List<PostDto>>(posts);
    }

    public async Task PublishPostAsync(int postId)
    {
        var post = await _posts.GetByIdAsync(postId)
            ?? throw new NotFoundException(nameof(Post), postId);

        if (post.PublishedAt != null) return; // اتنشر قبل كده

        var result = await _publisher.PublishAsync(_mapper.Map<PostDto>(post));
        if (!result.Success)
            throw new InvalidOperationException(result.ErrorMessage); // Hangfire هيعيد المحاولة

        post.PublishedAt = DateTime.UtcNow;
        await _posts.UpdateAsync(post);
    }
}
