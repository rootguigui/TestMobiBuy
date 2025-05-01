namespace TestMobiBuy.Domain.Settings;

public class RabbitMqSettings
{
    public const string SectionName = "RabbitMQ";
    
    public string Host { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = string.Empty;
} 