using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using TestMobiBuy.Application.Externals;
using TestMobiBuy.Application.Models.External;
using TestMobiBuy.Domain.Settings;
using Xunit;

namespace TestMobiBuy.UnitTests.Externals;

public class ViaCepApiExternalTests
{
    private readonly Mock<IOptions<ExternalServicesSettings>> _mockOptions;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly ViaCepApiExternal _viaCepApiExternal;

    public ViaCepApiExternalTests()
    {
        _mockOptions = new Mock<IOptions<ExternalServicesSettings>>();
        _mockOptions.Setup(x => x.Value)
            .Returns(new ExternalServicesSettings { ViaCepApiUrl = "https://viacep.com.br/ws" });

        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _viaCepApiExternal = new ViaCepApiExternal(_httpClient, _mockOptions.Object);
    }

    [Fact]
    public async Task GetAddressByZipCodeAsync_ShouldReturnAddress_WhenZipCodeIsValid()
    {
        // Arrange
        var zipCode = "12345678";
        var expectedResponse = new ViaCepResponseModel
        {
            Cep = "12345-678",
            Logradouro = "Rua Teste",
            Complemento = "Apto 101",
            Bairro = "Centro",
            Localidade = "São Paulo",
            Uf = "SP"
        };

        var responseContent = JsonSerializer.Serialize(expectedResponse);
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => 
                    req.Method == HttpMethod.Get && 
                    req.RequestUri.ToString() == $"https://viacep.com.br/ws/{zipCode}/json"),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _viaCepApiExternal.GetAddressByZipCodeAsync(zipCode);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task GetAddressByZipCodeAsync_ShouldReturnNull_WhenZipCodeIsInvalid()
    {
        // Arrange
        var zipCode = "00000000";
        var responseContent = "{\"erro\": \"true\"}";
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => 
                    req.Method == HttpMethod.Get && 
                    req.RequestUri.ToString() == $"https://viacep.com.br/ws/{zipCode}/json"),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _viaCepApiExternal.GetAddressByZipCodeAsync(zipCode);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAddressByZipCodeAsync_ShouldReturnNull_WhenRequestFails()
    {
        // Arrange
        var zipCode = "12345678";
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => 
                    req.Method == HttpMethod.Get && 
                    req.RequestUri.ToString() == $"https://viacep.com.br/ws/{zipCode}/json"),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _viaCepApiExternal.GetAddressByZipCodeAsync(zipCode);

        // Assert
        result.Should().BeNull();
    }
} 