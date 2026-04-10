namespace ULife.MongoApi.DTOs.Common;

public class CommentQueryParams
{
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 10;
    public string? PostId { get; set; }
    public string? ParentCommentId { get; set; }
    public string SortBy { get; set; } = "created_at";
    public string SortOrder { get; set; } = "desc";
}