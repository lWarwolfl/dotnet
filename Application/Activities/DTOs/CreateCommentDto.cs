namespace Application.Activities.DTOs;

public class CreateCommentDto : BaseCommentDto
{
    public required string ActivityId { get; set; }
}