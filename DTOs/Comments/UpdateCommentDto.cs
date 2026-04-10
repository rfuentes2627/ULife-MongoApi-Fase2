namespace ULife.MongoApi.DTOs.Comments;

public class UpdateCommentDto
{
    public string? Content { get; set; }
    public bool? IsAccepted { get; set; }
}