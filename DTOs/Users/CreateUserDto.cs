namespace ULife.MongoApi.DTOs.Users;

public class CreateUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string University { get; set; } = string.Empty;
    public string Career { get; set; } = string.Empty;
    public List<string> Skills { get; set; } = [];
    public List<string> Interests { get; set; } = [];
}