namespace API.DTOs;

public class UserInfoDto
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public string? Bio { get; set; }
    public required string DisplayName { get; set; }
    public string? ImageUrl { get; set; }
}