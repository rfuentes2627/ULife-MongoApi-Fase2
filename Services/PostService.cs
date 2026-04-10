using MongoDB.Bson;
using MongoDB.Driver;
using ULife.MongoApi.Data;
using ULife.MongoApi.DTOs.Common;
using ULife.MongoApi.DTOs.Posts;
using ULife.MongoApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ULife.MongoApi.Services;

public class PostService
{
    private readonly MongoContext _context;

    public PostService(MongoContext context)
    {
        _context = context;
    }

    public async Task<Post> CreateAsync(CreatePostDto dto)
    {
        var post = new Post
        {
            Author = dto.Author,
            Type = dto.Type,
            Content = dto.Content,
            Media = dto.Media ?? [],
            Tags = dto.Tags ?? [],
            Category = dto.Category,
            Visibility = dto.Visibility,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        await _context.Posts.InsertOneAsync(post);
        return post;
    }

    public async Task<PagedResult<Post>> GetAllAsync(PostQueryParams query)
    {
        var page = query.Page <= 0 ? 1 : query.Page;
        var limit = query.Limit <= 0 ? 10 : query.Limit;

        var filterBuilder = Builders<Post>.Filter;
        var filters = new List<FilterDefinition<Post>>();

        if (!string.IsNullOrWhiteSpace(query.Type))
            filters.Add(filterBuilder.Eq(x => x.Type, query.Type));

        if (!string.IsNullOrWhiteSpace(query.Category))
            filters.Add(filterBuilder.Eq(x => x.Category, query.Category));

        if (!string.IsNullOrWhiteSpace(query.Tag))
            filters.Add(filterBuilder.AnyEq(x => x.Tags, query.Tag));

        if (!string.IsNullOrWhiteSpace(query.AuthorId))
            filters.Add(filterBuilder.Eq("author.id", query.AuthorId));

        if (!string.IsNullOrWhiteSpace(query.Visibility))
            filters.Add(filterBuilder.Eq(x => x.Visibility, query.Visibility));

        var filter = filters.Count > 0
            ? filterBuilder.And(filters)
            : filterBuilder.Empty;

        var totalRecords = await _context.Posts.CountDocumentsAsync(filter);

        var sortField = string.IsNullOrWhiteSpace(query.SortBy) ? "created_at" : query.SortBy;
        var sortOrder = query.SortOrder?.ToLower() == "asc" ? "asc" : "desc";

        var sortDefinition = sortOrder == "asc"
            ? Builders<Post>.Sort.Ascending(sortField)
            : Builders<Post>.Sort.Descending(sortField);

        var data = await _context.Posts
            .Find(filter)
            .Sort(sortDefinition)
            .Skip((page - 1) * limit)
            .Limit(limit)
            .ToListAsync();

        return new PagedResult<Post>
        {
            Data = data,
            TotalRecords = totalRecords,
            Page = page,
            Limit = limit
        };
    }

    public async Task<Post?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return null;

        return await _context.Posts.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(string id, UpdatePostDto dto)
    {
        if (!ObjectId.TryParse(id, out _))
            return false;

        var updates = new List<UpdateDefinition<Post>>();

        if (!string.IsNullOrWhiteSpace(dto.Content))
            updates.Add(Builders<Post>.Update.Set(x => x.Content, dto.Content));

        if (dto.Media is not null)
            updates.Add(Builders<Post>.Update.Set(x => x.Media, dto.Media));

        if (dto.Tags is not null)
            updates.Add(Builders<Post>.Update.Set(x => x.Tags, dto.Tags));

        if (!string.IsNullOrWhiteSpace(dto.Category))
            updates.Add(Builders<Post>.Update.Set(x => x.Category, dto.Category));

        if (!string.IsNullOrWhiteSpace(dto.Visibility))
            updates.Add(Builders<Post>.Update.Set(x => x.Visibility, dto.Visibility));

        if (dto.IsPinned.HasValue)
            updates.Add(Builders<Post>.Update.Set(x => x.IsPinned, dto.IsPinned.Value));

        updates.Add(Builders<Post>.Update.Set(x => x.UpdatedAt, DateTime.UtcNow));

        var updateDefinition = Builders<Post>.Update.Combine(updates);

        var result = await _context.Posts.UpdateOneAsync(
            x => x.Id == id,
            updateDefinition);

        return result.MatchedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return false;

        var result = await _context.Posts.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }
}