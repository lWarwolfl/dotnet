using Application.Profiles.DTOs;

namespace Application.Activities.DTOs;

public class GetActivityDto : BaseActivityDto
{
    public required string Id { get; set; }
    public bool IsCancelled { get; set; }
    public required string HostId { get; set; }
    public ICollection<ProfileDto> Attendees { get; set; } = [];
}