namespace ULife.MongoApi.DTOs.Common;

public class PostQueryParams
{
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 10;
    public string? Type { get; set; }
    public string? Category { get; set; }
    public string? Tag { get; set; }
    public string? AuthorId { get; set; }
    public string? Visibility { get; set; }
    public string SortBy { get; set; } = "created_at";
    public string SortOrder { get; set; } = "desc";
}