using TestMobiBuy.Application.Models.External;

namespace TestMobiBuy.Application.Interfaces;

public interface IViaCepApiExternal
{
    Task<ViaCepResponseModel?> GetAddressByZipCodeAsync(string zipCode);
}
