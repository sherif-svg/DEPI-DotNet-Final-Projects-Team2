using FluentValidation;
using SocialMediaManager.Application.DTOs.Campaigns;

namespace SocialMediaManager.Application.Validators;

public class UpdateCampaignValidator : AbstractValidator<UpdateCampaignDto>
{
    public UpdateCampaignValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.Tone).IsInEnum();
        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.EndDate.HasValue);
    }
}
