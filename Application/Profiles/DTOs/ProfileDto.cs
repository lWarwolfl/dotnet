namespace Application.Profiles.DTOs;

public class ProfileDto
{
    public required string Id { get; set; }
    public string? Bio { get; set; }
    public required string DisplayName { get; set; }
    public string? ImageUrl { get; set; }
    public bool Following { get; set; }
    public int FollowerCount { get; set; }
    public int FollowingCount { get; set; }
}