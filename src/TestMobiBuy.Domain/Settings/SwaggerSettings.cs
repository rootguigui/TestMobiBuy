namespace TestMobiBuy.Domain.Settings;

public class SwaggerSettings
{
    public const string SectionName = "SwaggerSettings";
    public string Title { get; set; } = "API";
    public string Version { get; set; } = "v1";
    public string Description { get; set; } = string.Empty;
    public SwaggerSecuritySettings Security { get; set; } = new();
}

public class SwaggerSecuritySettings
{
    public const string SectionName = "SwaggerSettings:Security";
    public bool Enabled { get; set; } = false;
    public string Scheme { get; set; } = "Bearer";
}
