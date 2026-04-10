namespace ULife.MongoApi.DTOs.Users;

public class UpdateUserDto
{
    public string? FullName { get; set; }
    public string? University { get; set; }
    public string? Career { get; set; }
    public List<string>? Skills { get; set; }
    public List<string>? Interests { get; set; }
}