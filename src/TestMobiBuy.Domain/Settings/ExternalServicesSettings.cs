namespace TestMobiBuy.Domain.Settings;

public class ExternalServicesSettings
{
    public const string SectionName = "ExternalServices";
    public string ViaCepApiUrl { get; set; } = string.Empty;
}
