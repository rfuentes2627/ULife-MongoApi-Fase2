using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ULife.MongoApi.Models;

public class Comment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("post_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string PostId { get; set; } = string.Empty;

    [BsonElement("parent_comment_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ParentCommentId { get; set; }

    [BsonElement("author")]
    public EmbeddedAuthor Author { get; set; } = new();

    [BsonElement("content")]
    public string Content { get; set; } = string.Empty;

    [BsonElement("reactions_count")]
    public ReactionCount ReactionsCount { get; set; } = new();

    [BsonElement("replies_count")]
    public int RepliesCount { get; set; } = 0;

    [BsonElement("is_accepted")]
    public bool IsAccepted { get; set; } = false;

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}