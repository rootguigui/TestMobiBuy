using TestMobiBuy.Application.Dtos;

namespace TestMobiBuy.Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto?> GetByIdAsync(int customerId);
    Task<CustomerDto?> CreateAsync(CustomerCreateDto customerCreateDto);
    Task<CustomerDto?> UpdateAsync(int customerId, CustomerUpdateDto customerUpdateDto);
}
