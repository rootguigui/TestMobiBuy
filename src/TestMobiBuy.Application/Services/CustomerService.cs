using System.Net.Sockets;
using System.Text.Json;
using TestMobiBuy.Application.Dtos;
using TestMobiBuy.Application.Interfaces;
using TestMobiBuy.Application.Mappers;
using TestMobiBuy.Domain.Entities;
using TestMobiBuy.Domain.Exceptions;
using TestMobiBuy.Domain.Interfaces;

namespace TestMobiBuy.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerAddressRepository _customerAddressRepository;
    private readonly IViaCepApiExternal _viaCepApiExternal;
    private readonly IMessageBusService _messageBusService;

    public CustomerService
    (
        ICustomerRepository customerRepository,
        ICustomerAddressRepository customerAddressRepository,
        IViaCepApiExternal viaCepApiExternal,
        IMessageBusService messageBusService
    )
    {
        _customerRepository = customerRepository;
        _customerAddressRepository = customerAddressRepository;
        _viaCepApiExternal = viaCepApiExternal;
        _messageBusService = messageBusService;
    }

    public async Task<CustomerDto?> GetByIdAsync(int customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer is null) throw new DomainErrorException("Cliente não encontrado!");

        var addresses = await _customerAddressRepository.GetByCustomerIdAsync(customerId);
        customer!.Addresses = addresses.ToList();

        return customer?.ToDto();
    }

    public async Task<CustomerDto?> CreateAsync(CustomerCreateDto customerCreateDto)
    {
        var customer = await _customerRepository.GetByEmailAsync(customerCreateDto.Email);
        if (customer is not null) throw new DomainErrorException("E-mail já cadastrado!");

        var address = await _viaCepApiExternal.GetAddressByZipCodeAsync(customerCreateDto.ZipCode);
        if (address is null) throw new DomainErrorException("CEP não encontrado!");

        var newCustomer = new CustomerEntity
        {
            Name = customerCreateDto.Name,
            Email = customerCreateDto.Email,
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        await _customerRepository.AddAsync(newCustomer);
        await _customerRepository.SaveChangesAsync();

        var newCustomerAddress = new CustomerAddressEntity
        {
            CustomerId = newCustomer.Id,
            IsActive = true,
            Address = address.Logradouro,
            City = address.Localidade,
            State = address.Uf,
            ZipCode = address.Cep,
            Neighborhood = address.Bairro,
            Complement = address.Complemento,
            CreatedAt = DateTime.Now
        };

        await _customerAddressRepository.AddAsync(newCustomerAddress);
        await _customerAddressRepository.SaveChangesAsync();

        await _messageBusService.Publish(customerCreateDto);

        return newCustomer?.ToDto();
    }

    public async Task<CustomerDto?> UpdateAsync(int customerId, CustomerUpdateDto customerUpdateDto)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer is null) throw new DomainErrorException("Cliente não encontrado!");

        customer.Name = customerUpdateDto.Name;
        customer.Email = customerUpdateDto.Email;
        customer.UpdatedAt = DateTime.Now;

        await _customerRepository.UpdateAsync(customer);
        await _customerRepository.SaveChangesAsync();

        foreach (var address in customerUpdateDto.Addresses)
        {
            var customerAddress = await _customerAddressRepository.GetByIdAsync(address.CustomerAddressId);
            if (customerAddress is null) throw new DomainErrorException("Endereço do Cliente não encontrado!");

            var addressInformation = await _viaCepApiExternal.GetAddressByZipCodeAsync(address.ZipCode);
            if (addressInformation is null) throw new DomainErrorException("CEP não encontrado!");

            customerAddress.Address = addressInformation.Logradouro;
            customerAddress.City = addressInformation.Localidade;
            customerAddress.State = addressInformation.Uf;
            customerAddress.ZipCode = addressInformation.Cep;
            customerAddress.Neighborhood = addressInformation.Bairro;
            customerAddress.Complement = addressInformation.Complemento;
            customerAddress.UpdatedAt = DateTime.Now;

            await _customerAddressRepository.UpdateAsync(customerAddress);
            await _customerAddressRepository.SaveChangesAsync();
        }

        return customer?.ToDto();
    }
}
