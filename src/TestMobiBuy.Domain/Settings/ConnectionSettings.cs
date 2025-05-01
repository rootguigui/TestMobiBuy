namespace TestMobiBuy.Domain.Settings;

public class ConnectionSettings
{
    public const string SectionName = "ConnectionStrings";
    public string DefaultConnection { get; set; } = string.Empty;
}
