using AutoMapper;
using SocialMediaManager.Application.DTOs.Campaigns;
using SocialMediaManager.Application.DTOs.Posts;
using SocialMediaManager.Domain.Entities;

namespace SocialMediaManager.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateCampaignDto, Campaign>();
        CreateMap<UpdateCampaignDto, Campaign>();
        CreateMap<Campaign, CampaignDto>()
            .ForMember(d => d.PostsCount, o => o.MapFrom(s => s.Posts.Count));

        CreateMap<SchedulePostDto, Post>();
        CreateMap<Post, PostDto>();
    }
}
