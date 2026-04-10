namespace ULife.MongoApi.Models;

public class EmbeddedAuthor
{
    public string? Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string ProfilePictureUrl { get; set; } = string.Empty;
}