namespace ULife.MongoApi.Settings;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string UsersCollectionName { get; set; } = string.Empty;
    public string PostsCollectionName { get; set; } = string.Empty;
    public string CommentsCollectionName { get; set; } = string.Empty;
}