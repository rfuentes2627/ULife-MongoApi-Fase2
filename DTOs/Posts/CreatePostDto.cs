using ULife.MongoApi.Models;

namespace ULife.MongoApi.DTOs.Posts;

public class CreatePostDto
{
    public EmbeddedAuthor Author { get; set; } = new();
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<MediaItem> Media { get; set; } = [];
    public List<string> Tags { get; set; } = [];
    public string Category { get; set; } = string.Empty;
    public string Visibility { get; set; } = "public";
}