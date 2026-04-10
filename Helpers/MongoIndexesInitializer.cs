using MongoDB.Driver;
using ULife.MongoApi.Data;
using ULife.MongoApi.Models;

namespace ULife.MongoApi.Helpers;

public class MongoIndexesInitializer
{
    private readonly MongoContext _context;

    public MongoIndexesInitializer(MongoContext context)
    {
        _context = context;
    }

    public async Task CreateIndexesAsync()
    {
        var userIndexes = new List<CreateIndexModel<User>>
        {
            new(Builders<User>.IndexKeys.Ascending(x => x.Username), new CreateIndexOptions { Unique = true }),
            new(Builders<User>.IndexKeys.Ascending(x => x.Email), new CreateIndexOptions { Unique = true }),
            new(Builders<User>.IndexKeys.Ascending(x => x.University)),
            new(Builders<User>.IndexKeys.Ascending(x => x.Career))
        };

        var postIndexes = new List<CreateIndexModel<Post>>
        {
            new(Builders<Post>.IndexKeys.Ascending("author.id")),
            new(Builders<Post>.IndexKeys.Ascending(x => x.Type)),
            new(Builders<Post>.IndexKeys.Ascending(x => x.Category)),
            new(Builders<Post>.IndexKeys.Ascending(x => x.Tags)),
            new(Builders<Post>.IndexKeys.Descending(x => x.CreatedAt))
        };

        var commentIndexes = new List<CreateIndexModel<Comment>>
        {
            new(Builders<Comment>.IndexKeys.Ascending(x => x.PostId)),
            new(Builders<Comment>.IndexKeys.Ascending(x => x.ParentCommentId)),
            new(Builders<Comment>.IndexKeys.Descending(x => x.CreatedAt))
        };

        await _context.Users.Indexes.CreateManyAsync(userIndexes);
        await _context.Posts.Indexes.CreateManyAsync(postIndexes);
        await _context.Comments.Indexes.CreateManyAsync(commentIndexes);
    }
}