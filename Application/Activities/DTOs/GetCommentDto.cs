namespace Application.Activities.DTOs;

public class GetCommentDto : BaseCommentDto
{
    public required string Id { get; set; }
    public required string UserId { get; set; }
    public required string DisplayName { get; set; }
    public string? ImageUrl { get; set; }
}