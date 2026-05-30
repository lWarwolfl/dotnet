namespace Domain;

public class Image
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string PublicId { get; set; }
    public required string Url { get; set; }
}
