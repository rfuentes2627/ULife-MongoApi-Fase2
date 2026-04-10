using MongoDB.Bson;
using MongoDB.Driver;
using ULife.MongoApi.Data;
using ULife.MongoApi.DTOs.Common;
using ULife.MongoApi.DTOs.Users;
using ULife.MongoApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ULife.MongoApi.Services;

public class UserService
{
    private readonly MongoContext _context;

    public UserService(MongoContext context)
    {
        _context = context;
    }

    public async Task<User> CreateAsync(CreateUserDto dto)
    {
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = dto.PasswordHash,
            FullName = dto.FullName,
            University = dto.University,
            Career = dto.Career,
            Skills = dto.Skills ?? [],
            Interests = dto.Interests ?? [],
            ConnectionsCount = 0,
            PostsCount = 0,
            IsVerified = false,
            CreatedAt = DateTime.UtcNow,
            LastLogin = null,
            Settings = new UserSettings()
        };

        await _context.Users.InsertOneAsync(user);
        return user;
    }

    public async Task<PagedResult<User>> GetAllAsync(UserQueryParams query)
    {
        var page = query.Page <= 0 ? 1 : query.Page;
        var limit = query.Limit <= 0 ? 10 : query.Limit;

        var filterBuilder = Builders<User>.Filter;
        var filters = new List<FilterDefinition<User>>();

        if (!string.IsNullOrWhiteSpace(query.University))
            filters.Add(filterBuilder.Eq(x => x.University, query.University));

        if (!string.IsNullOrWhiteSpace(query.Career))
            filters.Add(filterBuilder.Eq(x => x.Career, query.Career));

        if (!string.IsNullOrWhiteSpace(query.Username))
            filters.Add(filterBuilder.Regex(
                x => x.Username,
                new BsonRegularExpression(query.Username, "i")));

        var filter = filters.Count > 0
            ? filterBuilder.And(filters)
            : filterBuilder.Empty;

        var totalRecords = await _context.Users.CountDocumentsAsync(filter);

        var sortField = string.IsNullOrWhiteSpace(query.SortBy) ? "created_at" : query.SortBy;
        var sortOrder = query.SortOrder?.ToLower() == "asc" ? "asc" : "desc";

        var sortDefinition = sortOrder == "asc"
            ? Builders<User>.Sort.Ascending(sortField)
            : Builders<User>.Sort.Descending(sortField);

        var data = await _context.Users
            .Find(filter)
            .Sort(sortDefinition)
            .Skip((page - 1) * limit)
            .Limit(limit)
            .ToListAsync();

        return new PagedResult<User>
        {
            Data = data,
            TotalRecords = totalRecords,
            Page = page,
            Limit = limit
        };
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return null;

        return await _context.Users.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(string id, UpdateUserDto dto)
    {
        if (!ObjectId.TryParse(id, out _))
            return false;

        var updates = new List<UpdateDefinition<User>>();

        if (!string.IsNullOrWhiteSpace(dto.FullName))
            updates.Add(Builders<User>.Update.Set(x => x.FullName, dto.FullName));

        if (!string.IsNullOrWhiteSpace(dto.University))
            updates.Add(Builders<User>.Update.Set(x => x.University, dto.University));

        if (!string.IsNullOrWhiteSpace(dto.Career))
            updates.Add(Builders<User>.Update.Set(x => x.Career, dto.Career));

        if (dto.Skills is not null)
            updates.Add(Builders<User>.Update.Set(x => x.Skills, dto.Skills));

        if (dto.Interests is not null)
            updates.Add(Builders<User>.Update.Set(x => x.Interests, dto.Interests));

        if (updates.Count == 0)
            return false;

        var updateDefinition = Builders<User>.Update.Combine(updates);

        var result = await _context.Users.UpdateOneAsync(
            x => x.Id == id,
            updateDefinition);

        return result.MatchedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return false;

        var result = await _context.Users.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }
}