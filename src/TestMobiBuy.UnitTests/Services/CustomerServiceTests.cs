using AutoFixture;
using FluentAssertions;
using Moq;
using TestMobiBuy.Application.Dtos;
using TestMobiBuy.Application.Interfaces;
using TestMobiBuy.Application.Models.External;
using TestMobiBuy.Application.Models.Request;
using TestMobiBuy.Application.Models.Response;
using TestMobiBuy.Application.Services;
using TestMobiBuy.Domain.Entities;
using TestMobiBuy.Domain.Exceptions;
using TestMobiBuy.Domain.Interfaces;
using TestMobiBuy.Infrastructure.Repositories;
using Xunit;

namespace TestMobiBuy.UnitTests.Services;

public class CustomerServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<ICustomerAddressRepository> _customerAddressRepositoryMock;
    private readonly Mock<IViaCepApiExternal> _viaCepApiExternalMock;
    private readonly Mock<IMessageBusService> _messageBusServiceMock;
    private readonly ICustomerService _customerService;

    public CustomerServiceTests()
    {
        _fixture = new Fixture();
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _customerAddressRepositoryMock = new Mock<ICustomerAddressRepository>();
        _viaCepApiExternalMock = new Mock<IViaCepApiExternal>();
        _messageBusServiceMock = new Mock<IMessageBusService>();
        _customerService = new CustomerService(_customerRepositoryMock.Object, _customerAddressRepositoryMock.Object, _viaCepApiExternalMock.Object, _messageBusServiceMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCustomerDto()
    {
        // Arrange
        var customerCreateDto = _fixture.Create<CustomerCreateDto>();
        var customer = new CustomerEntity();

        _customerRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CustomerEntity>()))
            .ReturnsAsync(customer);

        _viaCepApiExternalMock.Setup(x => x.GetAddressByZipCodeAsync(It.IsAny<string>())).ReturnsAsync(new ViaCepResponseModel());
        // Act
        var result = await _customerService.CreateAsync(customerCreateDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CustomerDto>();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowDomainErrorException_WhenEmailExists()
    {
        // Arrange
        var customerCreateDto = _fixture.Create<CustomerCreateDto>();
        _customerRepositoryMock.Setup(x => x.GetByEmailAsync(customerCreateDto.Email))
            .ReturnsAsync(new CustomerEntity());

        // Act & Assert
        await Assert.ThrowsAsync<DomainErrorException>(() => 
            _customerService.CreateAsync(customerCreateDto));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCustomerDto()
    {
        // Arrange
        var customerId = _fixture.Create<int>();
        var customer = new CustomerEntity();
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customerId))
            .ReturnsAsync(customer);

        // Act
        var result = await _customerService.GetByIdAsync(customerId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CustomerDto>();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowDomainErrorException_WhenCustomerNotFound()
    {
        // Arrange
        var customerId = _fixture.Create<int>();
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customerId))
            .ReturnsAsync((CustomerEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<DomainErrorException>(() => 
            _customerService.GetByIdAsync(customerId));
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnCustomerDto()
    {
        // Arrange
        var customerId = _fixture.Create<int>();
        var customerUpdateDto = _fixture.Create<CustomerUpdateDto>();
        var customer = new CustomerEntity();

        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customerId))
            .ReturnsAsync(customer);
        
        _customerRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CustomerEntity>()))
            .ReturnsAsync(customer);
        
        _customerAddressRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new CustomerAddressEntity());

        _viaCepApiExternalMock.Setup(x => x.GetAddressByZipCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(new ViaCepResponseModel());

        // Act
        var result = await _customerService.UpdateAsync(customerId, customerUpdateDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CustomerDto>();
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowDomainErrorException_WhenCustomerNotFound()
    {
        // Arrange
        var customerId = _fixture.Create<int>();
        var customerUpdateDto = _fixture.Create<CustomerUpdateDto>();
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customerId))
            .ReturnsAsync((CustomerEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<DomainErrorException>(() => 
            _customerService.UpdateAsync(customerId, customerUpdateDto));
    }
} 