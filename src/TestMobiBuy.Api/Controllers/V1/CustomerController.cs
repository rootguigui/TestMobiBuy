using Microsoft.AspNetCore.Mvc;
using TestMobiBuy.Application.Models.Response;
using TestMobiBuy.Application.Interfaces;
using TestMobiBuy.Application.Mappers;
using TestMobiBuy.Application.Models.Request;
using System.Net.Mime;
using TestMobiBuy.Domain.Exceptions;

namespace TestMobiBuy.Api.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
[Consumes(MediaTypeNames.Application.Json, "application/json")]
[Produces(MediaTypeNames.Application.Json, "application/json")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("GetById/{customerId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerResponseModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerResponseModel>> GetById(int customerId)
    {
        try
        {
            var customer = await _customerService.GetByIdAsync(customerId);

            return Ok(customer!.ToResponse());
        }
        catch (DomainErrorException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CustomerResponseModel>> Create([FromBody] CustomerCreateRequestModel customerCreateRequestModel)
    {
        try
        {
            var customer = await _customerService.CreateAsync(customerCreateRequestModel.ToDto());

            return Ok(customer!.ToResponse());
        }
        catch (DomainErrorException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("Update/{customerId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerResponseModel>> UpdateAsync
    (
        [FromRoute] int customerId, 
        [FromBody] CustomerUpdateRequestModel customerUpdateRequestModel
    )
    {
        try
        {
            var customer = await _customerService.UpdateAsync(customerId, customerUpdateRequestModel.ToDto());

            return Ok(customer!.ToResponse());
        }
        catch (DomainErrorException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
