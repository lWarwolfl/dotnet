namespace Application.Activities.DTOs;

public class BaseCommentDto
{
    public required string Body { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}