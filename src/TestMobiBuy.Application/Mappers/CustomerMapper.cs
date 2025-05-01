using Mapster;
using TestMobiBuy.Application.Dtos;
using TestMobiBuy.Application.Models.Request;
using TestMobiBuy.Application.Models.Response;
using TestMobiBuy.Domain.Entities;

namespace TestMobiBuy.Application.Mappers;

public static class CustomerMapper
{
    private static TypeAdapterConfig? configModelToDto = null;
    private static TypeAdapterConfig? configDtoToModel = null;
    private static TypeAdapterConfig? configEntityToDto = null;

    public static CustomerResponseModel ToResponse(this CustomerDto customer)
    {
        configDtoToModel ??= GetConfigDtoToModel();

        return customer.Adapt<CustomerResponseModel>(configDtoToModel);
    }

    public static CustomerDto ToDto(this CustomerResponseModel customer)
    {
        configModelToDto ??= GetConfigModelToDto();

        return customer.Adapt<CustomerDto>(configModelToDto);
    }

    public static CustomerDto ToDto(this CustomerEntity customer)
    {
        configEntityToDto ??= GetConfigEntityToDto();

        return customer.Adapt<CustomerDto>(configEntityToDto);
    }

    public static CustomerCreateDto ToDto(this CustomerCreateRequestModel customerCreateRequestModel)
    {
        return customerCreateRequestModel.Adapt<CustomerCreateDto>();
    }

    public static CustomerUpdateDto ToDto(this CustomerUpdateRequestModel customerUpdateRequestModel)
    {
        var config = new TypeAdapterConfig()
            .NewConfig<CustomerUpdateRequestModel, CustomerUpdateDto>()
            .Map(dest => dest.Addresses, src => src.Addresses.Select(x => 
                new CustomerAddressUpdateDto {
                    CustomerAddressId = x.CustomerAddressId,
                    ZipCode = x.ZipCode
                }))
            .Config;

        return customerUpdateRequestModel.Adapt<CustomerUpdateDto>(config);
    }

    private static TypeAdapterConfig GetConfigModelToDto()
    {
        return new TypeAdapterConfig()
            .NewConfig<CustomerResponseModel, CustomerDto>()
            .Config;
    }

    private static TypeAdapterConfig GetConfigDtoToModel()
    {
        return new TypeAdapterConfig()
            .NewConfig<CustomerDto, CustomerResponseModel>()
            .Config;
    }

    private static TypeAdapterConfig GetConfigEntityToDto()
    {
        return new TypeAdapterConfig()
            .NewConfig<CustomerEntity, CustomerDto>()
            .Map(dest => dest.CustomerId, src => src.Id)
            .Map(dest => dest.Addresses, src => src.Addresses.Select(x => 
                new CustomerAddressDto {
                    CustomerAddressId = x.Id,
                    Address = x.Address,
                    City = x.City,
                    State = x.State,
                    ZipCode = x.ZipCode,
                    Neighborhood = x.Neighborhood,
                }))
            .Config;
    }
}
