using System.Text.Json;
using Microsoft.Extensions.Options;
using TestMobiBuy.Application.Interfaces;
using TestMobiBuy.Application.Models.External;
using TestMobiBuy.Domain.Settings;

namespace TestMobiBuy.Application.Externals;

public class ViaCepApiExternal : IViaCepApiExternal
{
    private readonly HttpClient _httpClient;
    private readonly ExternalServicesSettings _externalServicesSettings;

    public ViaCepApiExternal(HttpClient httpClient, IOptions<ExternalServicesSettings> externalServicesSettings)
    {
        _httpClient = httpClient;
        _externalServicesSettings = externalServicesSettings.Value ?? new();
    }

    public async Task<ViaCepResponseModel?> GetAddressByZipCodeAsync(string zipCode)
    {
        var response = await _httpClient.GetAsync($"{_externalServicesSettings.ViaCepApiUrl}/{zipCode}/json");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            
            // Verifica se a resposta contém o erro
            if (content.Contains("\"erro\": \"true\""))
            {
                return null;
            }
            
            return JsonSerializer.Deserialize<ViaCepResponseModel>(content);
        }

        return null;
    }
}

