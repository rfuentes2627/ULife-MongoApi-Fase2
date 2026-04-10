namespace ULife.MongoApi.Models;

public class UserSettings
{
    public bool EmailNotifications { get; set; } = true;
    public bool PushNotifications { get; set; } = true;
    public string Privacy { get; set; } = "public";
    public string Language { get; set; } = "es";
}