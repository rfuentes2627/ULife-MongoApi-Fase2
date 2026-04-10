namespace ULife.MongoApi.DTOs.Common;

public class UserQueryParams
{
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 10;
    public string? University { get; set; }
    public string? Career { get; set; }
    public string? Username { get; set; }
    public string SortBy { get; set; } = "created_at";
    public string SortOrder { get; set; } = "desc";
}