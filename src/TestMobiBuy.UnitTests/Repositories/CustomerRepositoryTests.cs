using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Moq;
using TestMobiBuy.Domain.Entities;
using TestMobiBuy.Infrastructure.Contexts;
using TestMobiBuy.Infrastructure.Repositories;
using Xunit;

namespace TestMobiBuy.UnitTests.Repositories;

public class CustomerRepositoryTests
{
    private readonly Fixture _fixture;
    private readonly DbContextOptions<DataContext> _options;
    private readonly DataContext _context;
    private readonly CustomerRepository _repository;

    public CustomerRepositoryTests()
    {
        _fixture = new Fixture();
        _options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestMobiBuyDb")
            .Options;
        _context = new DataContext(_options);
        _repository = new CustomerRepository(_context);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddCustomerToDatabase()
    {
        // Arrange
        var customer = new CustomerEntity { Name = "Teste", Email = "teste@teste.com", IsActive = true, CreatedAt = DateTime.Now };

        // Act
        var result = await _repository.AddAsync(customer);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(customer);
        var customerInDb = await _context.Customers.FindAsync(customer.Id);
        customerInDb.Should().NotBeNull();
        customerInDb.Should().BeEquivalentTo(customer);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenCustomerIsNull()
    {
        // Arrange
        CustomerEntity customer = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _repository.AddAsync(customer));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCustomer()
    {
        // Arrange  
        var customer = new CustomerEntity();
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(customer.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(customer);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCustomerNotFound()
    {
        // Arrange
        var customerId = _fixture.Create<int>();

        // Act
        var result = await _repository.GetByIdAsync(customerId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCustomerInDatabase()
    {
        // Arrange
        var customer = new CustomerEntity();
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        customer.Email = "teste1@teste.com";
        customer.Name = "Teste update";

        // Act
        var result = await _repository.UpdateAsync(customer);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(customer);
        
        var customerInDb = await _context.Customers.FindAsync(customer.Id);

        customerInDb.Should().NotBeNull();
        customerInDb.Should().BeEquivalentTo(customer);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnCustomer()
    {
        // Arrange
        var customer = new CustomerEntity();
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByEmailAsync(customer.Email);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenCustomerNotFound()
    {
        // Arrange
        var email = _fixture.Create<string>();

        // Act
        var result = await _repository.GetByEmailAsync(email);

        // Assert
        result.Should().BeNull();
    }
} 