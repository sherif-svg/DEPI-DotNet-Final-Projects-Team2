using FluentValidation;
using SocialMediaManager.Application.DTOs.Posts;

namespace SocialMediaManager.Application.Validators;

public class SchedulePostValidator : AbstractValidator<SchedulePostDto>
{
    public SchedulePostValidator()
    {
        RuleFor(x => x.CampaignId).GreaterThan(0);
        RuleFor(x => x.Content).NotEmpty().MaximumLength(3000);
        RuleFor(x => x.Platform).IsInEnum();
        RuleFor(x => x.ScheduledAt)
            .Must(d => d > DateTime.UtcNow)
            .WithMessage("Scheduled time must be in the future.");
    }
}
