using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ULife.MongoApi.Models;
using ULife.MongoApi.Settings;

namespace ULife.MongoApi.Data;

public class MongoContext
{
    private readonly IMongoDatabase _database;
    private readonly MongoDbSettings _settings;

    public MongoContext(IOptions<MongoDbSettings> settings)
    {
        _settings = settings.Value;
        var client = new MongoClient(_settings.ConnectionString);
        _database = client.GetDatabase(_settings.DatabaseName);
    }

    public IMongoCollection<User> Users =>
        _database.GetCollection<User>(_settings.UsersCollectionName);

    public IMongoCollection<Post> Posts =>
        _database.GetCollection<Post>(_settings.PostsCollectionName);

    public IMongoCollection<Comment> Comments =>
        _database.GetCollection<Comment>(_settings.CommentsCollectionName);
}