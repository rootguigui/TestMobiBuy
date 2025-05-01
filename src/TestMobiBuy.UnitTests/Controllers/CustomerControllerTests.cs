using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TestMobiBuy.Api.Controllers.V1;
using TestMobiBuy.Application.Dtos;
using TestMobiBuy.Application.Interfaces;
using TestMobiBuy.Application.Models.Request;
using TestMobiBuy.Application.Models.Response;
using TestMobiBuy.Domain.Exceptions;
using Xunit;

namespace TestMobiBuy.UnitTests.Controllers;

public class CustomerControllerTests
{
    private readonly Fixture _fixture;
    private readonly Mock<ICustomerService> _customerServiceMock;
    private readonly CustomerController _controller;
    private readonly Mock<HttpContext> _httpContextMock = new();
    private readonly Mock<HttpRequest> _httpRequestMock = new();

    public CustomerControllerTests()
    {
        _fixture = new Fixture();
        _customerServiceMock = new Mock<ICustomerService>();
        _controller = new CustomerController(_customerServiceMock.Object);
        _controller.ControllerContext.HttpContext = CreateHttpContextMock();
    }

    private HttpContext CreateHttpContextMock()
    {
        _httpRequestMock.Setup(r => r.Path).Returns("/v1/customer");
        _httpContextMock.Setup(c => c.Request).Returns(_httpRequestMock.Object);
        return _httpContextMock.Object;
    }


    [Fact]
    public async Task CreateCustomer_ShouldReturnCreated()
    {
        var customer = _fixture.Create<CustomerCreateRequestModel>();
        var response = new CustomerDto();
        _customerServiceMock.Setup(x => x.CreateAsync(It.IsAny<CustomerCreateDto>()))
            .ReturnsAsync(response);

        var result = await _controller.Create(customer);

        var successResult = result.Result as ObjectResult;
        successResult.Should().NotBeNull();
        successResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
        var successResponse = successResult?.Value.Should().BeAssignableTo<CustomerResponseModel>().Subject;
    }

    [Fact]
    public async Task CreateCustomer_ErrorResponse()
    {
        var request = _fixture.Create<CustomerCreateRequestModel>();
        var response = new CustomerDto();

        _customerServiceMock.Setup(x => x.CreateAsync(It.IsAny<CustomerCreateDto>())).Throws(() => new DomainErrorException("E-mail já cadastrado!"));

        var result = await _controller.Create(request);

        var errorResult = result.Result as ObjectResult;
        errorResult.Should().NotBeNull();
        errorResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task GetCustomerById_ShouldReturnOk()
    {
        var customerId = _fixture.Create<int>();
        var response = _fixture.Create<CustomerDto>();
        _customerServiceMock.Setup(x => x.GetByIdAsync(customerId))
            .ReturnsAsync(response);

        var result = await _controller.GetById(customerId);

        var successResult = result.Result as ObjectResult;
        successResult.Should().NotBeNull();
        successResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
        var successResponse = successResult?.Value.Should().BeAssignableTo<CustomerResponseModel>().Subject;
    }

    [Fact]
    public async Task GetCustomerById_ErrorResponse()
    {
        var customerId = _fixture.Create<int>();

        _customerServiceMock.Setup(x => x.GetByIdAsync(customerId)).Throws(() => new DomainErrorException("Cliente não encontrado!"));

        var result = await _controller.GetById(customerId);

        var errorResult = result.Result as ObjectResult;
        errorResult.Should().NotBeNull();
        errorResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task UpdateCustomer_ShouldReturnOk()
    {
        var customerId = _fixture.Create<int>();
        var customer = _fixture.Create<CustomerUpdateRequestModel>();
        var response = _fixture.Create<CustomerDto>();
        _customerServiceMock.Setup(x => x.UpdateAsync(customerId, It.IsAny<CustomerUpdateDto>()))
            .ReturnsAsync(response);

        var result = await _controller.UpdateAsync(customerId, customer);

        var successResult = result.Result as ObjectResult;
        successResult.Should().NotBeNull();
        successResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
        var successResponse = successResult?.Value.Should().BeAssignableTo<CustomerResponseModel>().Subject;
    }

    [Fact]
    public async Task UpdateCustomer_ErrorResponse()
    {
        var customerId = _fixture.Create<int>();
        var request = _fixture.Create<CustomerUpdateRequestModel>();

        _customerServiceMock.Setup(x => x.UpdateAsync(customerId, It.IsAny<CustomerUpdateDto>())).Throws(() => new DomainErrorException("Dados inválidos!"));

        var result = await _controller.UpdateAsync(customerId, request);

        var errorResult = result.Result as ObjectResult;
        errorResult.Should().NotBeNull();
        errorResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

}