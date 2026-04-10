using MongoDB.Bson;
using MongoDB.Driver;
using ULife.MongoApi.Data;
using ULife.MongoApi.DTOs.Comments;
using ULife.MongoApi.DTOs.Common;
using ULife.MongoApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ULife.MongoApi.Services;

public class CommentService
{
    private readonly MongoContext _context;

    public CommentService(MongoContext context)
    {
        _context = context;
    }

    public async Task<Comment?> CreateAsync(CreateCommentDto dto)
    {
        if (!ObjectId.TryParse(dto.PostId, out _))
            return null;

        if (!string.IsNullOrWhiteSpace(dto.ParentCommentId) &&
            !ObjectId.TryParse(dto.ParentCommentId, out _))
            return null;

        var postExists = await _context.Posts.Find(x => x.Id == dto.PostId).AnyAsync();
        if (!postExists)
            return null;

        var comment = new Comment
        {
            PostId = dto.PostId,
            ParentCommentId = dto.ParentCommentId,
            Author = dto.Author,
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Comments.InsertOneAsync(comment);

        await _context.Posts.UpdateOneAsync(
            x => x.Id == dto.PostId,
            Builders<Post>.Update.Inc(x => x.CommentsCount, 1));

        if (!string.IsNullOrWhiteSpace(dto.ParentCommentId))
        {
            await _context.Comments.UpdateOneAsync(
                x => x.Id == dto.ParentCommentId,
                Builders<Comment>.Update.Inc(x => x.RepliesCount, 1));
        }

        return comment;
    }

    public async Task<PagedResult<Comment>> GetAllAsync(CommentQueryParams query)
    {
        var page = query.Page <= 0 ? 1 : query.Page;
        var limit = query.Limit <= 0 ? 10 : query.Limit;

        var filterBuilder = Builders<Comment>.Filter;
        var filters = new List<FilterDefinition<Comment>>();

        if (!string.IsNullOrWhiteSpace(query.PostId))
        {
            if (!ObjectId.TryParse(query.PostId, out _))
            {
                return new PagedResult<Comment>
                {
                    Data = [],
                    TotalRecords = 0,
                    Page = page,
                    Limit = limit
                };
            }

            filters.Add(filterBuilder.Eq(x => x.PostId, query.PostId));
        }

        if (!string.IsNullOrWhiteSpace(query.ParentCommentId))
        {
            if (!ObjectId.TryParse(query.ParentCommentId, out _))
            {
                return new PagedResult<Comment>
                {
                    Data = [],
                    TotalRecords = 0,
                    Page = page,
                    Limit = limit
                };
            }

            filters.Add(filterBuilder.Eq(x => x.ParentCommentId, query.ParentCommentId));
        }

        var filter = filters.Count > 0
            ? filterBuilder.And(filters)
            : filterBuilder.Empty;

        var totalRecords = await _context.Comments.CountDocumentsAsync(filter);

        var sortField = string.IsNullOrWhiteSpace(query.SortBy) ? "created_at" : query.SortBy;
        var sortOrder = query.SortOrder?.ToLower() == "asc" ? "asc" : "desc";

        var sortDefinition = sortOrder == "asc"
            ? Builders<Comment>.Sort.Ascending(sortField)
            : Builders<Comment>.Sort.Descending(sortField);

        var data = await _context.Comments
            .Find(filter)
            .Sort(sortDefinition)
            .Skip((page - 1) * limit)
            .Limit(limit)
            .ToListAsync();

        return new PagedResult<Comment>
        {
            Data = data,
            TotalRecords = totalRecords,
            Page = page,
            Limit = limit
        };
    }

    public async Task<Comment?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return null;

        return await _context.Comments.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(string id, UpdateCommentDto dto)
    {
        if (!ObjectId.TryParse(id, out _))
            return false;

        var updates = new List<UpdateDefinition<Comment>>();

        if (!string.IsNullOrWhiteSpace(dto.Content))
            updates.Add(Builders<Comment>.Update.Set(x => x.Content, dto.Content));

        if (dto.IsAccepted.HasValue)
            updates.Add(Builders<Comment>.Update.Set(x => x.IsAccepted, dto.IsAccepted.Value));

        if (updates.Count == 0)
            return false;

        var updateDefinition = Builders<Comment>.Update.Combine(updates);

        var result = await _context.Comments.UpdateOneAsync(
            x => x.Id == id,
            updateDefinition);

        return result.MatchedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return false;

        var comment = await _context.Comments.Find(x => x.Id == id).FirstOrDefaultAsync();
        if (comment is null)
            return false;

        var result = await _context.Comments.DeleteOneAsync(x => x.Id == id);

        if (result.DeletedCount > 0)
        {
            await _context.Posts.UpdateOneAsync(
                x => x.Id == comment.PostId,
                Builders<Post>.Update.Inc(x => x.CommentsCount, -1));

            if (!string.IsNullOrWhiteSpace(comment.ParentCommentId))
            {
                await _context.Comments.UpdateOneAsync(
                    x => x.Id == comment.ParentCommentId,
                    Builders<Comment>.Update.Inc(x => x.RepliesCount, -1));
            }

            return true;
        }

        return false;
    }
}