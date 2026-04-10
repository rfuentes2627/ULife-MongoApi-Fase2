using ULife.MongoApi.Models;

namespace ULife.MongoApi.DTOs.Comments;

public class CreateCommentDto
{
    public string PostId { get; set; } = string.Empty;
    public string? ParentCommentId { get; set; }
    public EmbeddedAuthor Author { get; set; } = new();
    public string Content { get; set; } = string.Empty;
}