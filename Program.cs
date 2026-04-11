using MongoDB.Driver;
using ULife.MongoApi.Data;
using ULife.MongoApi.Services;
using ULife.MongoApi.Settings;
using ULife.MongoApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<MongoContext>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddSingleton<MongoIndexesInitializer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger("MongoStartup");

    try
    {
        var indexInitializer = scope.ServiceProvider.GetRequiredService<MongoIndexesInitializer>();
        await indexInitializer.CreateIndexesAsync();
        logger.LogInformation("MongoDB indexes were created or already exist.");
    }
    catch (TimeoutException ex)
    {
        logger.LogWarning(ex, "MongoDB is not reachable at startup. API will continue running, but database endpoints may fail until MongoDB is available.");
    }
    catch (MongoException ex)
    {
        logger.LogWarning(ex, "MongoDB initialization failed at startup. API will continue running, but database endpoints may fail until MongoDB is available.");
    }
}

app.Run();