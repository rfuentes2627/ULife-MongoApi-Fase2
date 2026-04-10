using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ULife.MongoApi.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("username")]
    public string Username { get; set; } = string.Empty;

    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [BsonElement("full_name")]
    public string FullName { get; set; } = string.Empty;

    [BsonElement("university")]
    public string University { get; set; } = string.Empty;

    [BsonElement("career")]
    public string Career { get; set; } = string.Empty;

    [BsonElement("skills")]
    public List<string> Skills { get; set; } = [];

    [BsonElement("interests")]
    public List<string> Interests { get; set; } = [];

    [BsonElement("connections_count")]
    public int ConnectionsCount { get; set; } = 0;

    [BsonElement("posts_count")]
    public int PostsCount { get; set; } = 0;

    [BsonElement("is_verified")]
    public bool IsVerified { get; set; } = false;

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("last_login")]
    public DateTime? LastLogin { get; set; }

    [BsonElement("settings")]
    public UserSettings Settings { get; set; } = new();
}