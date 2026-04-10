using ULife.MongoApi.Models;

namespace ULife.MongoApi.DTOs.Posts;

public class UpdatePostDto
{
    public string? Content { get; set; }
    public List<MediaItem>? Media { get; set; }
    public List<string>? Tags { get; set; }
    public string? Category { get; set; }
    public string? Visibility { get; set; }
    public bool? IsPinned { get; set; }
}