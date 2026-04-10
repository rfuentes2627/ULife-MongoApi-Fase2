using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ULife.MongoApi.Models;

public class Post
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("author")]
    public EmbeddedAuthor Author { get; set; } = new();

    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    [BsonElement("content")]
    public string Content { get; set; } = string.Empty;

    [BsonElement("media")]
    public List<MediaItem> Media { get; set; } = [];

    [BsonElement("tags")]
    public List<string> Tags { get; set; } = [];

    [BsonElement("category")]
    public string Category { get; set; } = string.Empty;

    [BsonElement("group_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? GroupId { get; set; }

    [BsonElement("visibility")]
    public string Visibility { get; set; } = "public";

    [BsonElement("reactions_count")]
    public ReactionCount ReactionsCount { get; set; } = new();

    [BsonElement("comments_count")]
    public int CommentsCount { get; set; } = 0;

    [BsonElement("shares_count")]
    public int SharesCount { get; set; } = 0;

    [BsonElement("views_count")]
    public int ViewsCount { get; set; } = 0;

    [BsonElement("is_pinned")]
    public bool IsPinned { get; set; } = false;

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}