namespace ULife.MongoApi.Models;

public class MediaItem
{
    public string Url { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public long Size { get; set; }
    public string AltText { get; set; } = string.Empty;
}