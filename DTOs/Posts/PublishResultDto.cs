namespace SocialMediaManager.Application.DTOs.Posts;

// نتيجة النشر على المنصة
public class PublishResultDto
{
    public bool Success { get; set; }
    public string? ExternalPostId { get; set; }
    public string? ErrorMessage { get; set; }
}
