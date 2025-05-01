using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TestMobiBuy.Domain.Entities;
using TestMobiBuy.Infrastructure.Contexts;
using TestMobiBuy.Infrastructure.Repositories;
using Xunit;

namespace TestMobiBuy.UnitTests.Repositories;

public class CustomerAddressRepositoryTests
{
    private readonly Fixture _fixture;
    private readonly DbContextOptions<DataContext> _options;
    private readonly DataContext _context;
    private readonly CustomerAddressRepository _repository;

    public CustomerAddressRepositoryTests()
    {
        _fixture = new Fixture();
        _options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestMobiBuyDb")
            .Options;
        _context = new DataContext(_options);
        _repository = new CustomerAddressRepository(_context);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddCustomerAddressToDatabase()
    {
        // Arrange
        var customerAddress = new CustomerAddressEntity 
        { 
            CustomerId = 1,
            Address = "Rua Teste",
            Number = "123",
            City = "São Paulo",
            State = "SP",
            ZipCode = "12345-678",
            Country = "Brasil",
            Neighborhood = "Centro",
            Complement = "Apto 101",
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        // Act
        var result = await _repository.AddAsync(customerAddress);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(customerAddress);
        var addressInDb = await _context.CustomerAddresses.FindAsync(customerAddress.Id);
        addressInDb.Should().NotBeNull();
        addressInDb.Should().BeEquivalentTo(customerAddress);
    }

    [Fact]
    public async Task GetByCustomerIdAsync_ShouldReturnCustomerAddresses()
    {
        // Arrange
        var customerId = 1;
        var addresses = new List<CustomerAddressEntity>
        {
            new CustomerAddressEntity 
            { 
                CustomerId = customerId,
                Address = "Rua Teste 1",
                Number = "123",
                City = "São Paulo",
                State = "SP",
                ZipCode = "12345-678",
                Country = "Brasil",
                Neighborhood = "Centro",
                Complement = "Apto 101",
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new CustomerAddressEntity 
            { 
                CustomerId = customerId,
                Address = "Rua Teste 2",
                Number = "456",
                City = "São Paulo",
                State = "SP",
                ZipCode = "12345-678",
                Country = "Brasil",
                Neighborhood = "Centro",
                Complement = "Apto 102",
                IsActive = true,
                CreatedAt = DateTime.Now
            }
        };

        await _context.CustomerAddresses.AddRangeAsync(addresses);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByCustomerIdAsync(customerId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetByCustomerIdAsync_ShouldReturnEmptyList_WhenNoAddressesFound()
    {
        // Arrange
        var customerId = 999;

        // Act
        var result = await _repository.GetByCustomerIdAsync(customerId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCustomerAddressInDatabase()
    {
        // Arrange
        var customerAddress = new CustomerAddressEntity 
        { 
            CustomerId = 1,
            Address = "Rua Teste",
            Number = "123",
            City = "São Paulo",
            State = "SP",
            ZipCode = "12345-678",
            Country = "Brasil",
            Neighborhood = "Centro",
            Complement = "Apto 101",
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        await _context.CustomerAddresses.AddAsync(customerAddress);
        await _context.SaveChangesAsync();

        customerAddress.Address = "Rua Atualizada";
        customerAddress.Number = "456";

        // Act
        var result = await _repository.UpdateAsync(customerAddress);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(customerAddress);
        var addressInDb = await _context.CustomerAddresses.FindAsync(customerAddress.Id);
        addressInDb.Should().NotBeNull();
        addressInDb.Should().BeEquivalentTo(customerAddress);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveCustomerAddressFromDatabase()
    {
        // Arrange
        var customerAddress = new CustomerAddressEntity 
        { 
            CustomerId = 1,
            Address = "Rua Teste",
            Number = "123",
            City = "São Paulo",
            State = "SP",
            ZipCode = "12345-678",
            Country = "Brasil",
            Neighborhood = "Centro",
            Complement = "Apto 101",
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        await _context.CustomerAddresses.AddAsync(customerAddress);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.DeleteAsync(customerAddress.Id);

        // Assert
        result.Should().BeTrue();
        var addressInDb = await _context.CustomerAddresses.FindAsync(customerAddress.Id);
        addressInDb.Should().BeNull();
    }
} 