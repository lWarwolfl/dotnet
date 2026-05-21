namespace Application.Profiles.DTOs;

public class ProfileDto
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public string? DisplayName { get; set; }
    public string? ImageUrl { get; set; }
}