using TestMobiBuy.Domain.Entities;

namespace TestMobiBuy.Domain.Interfaces;

public interface ICustomerRepository : IBaseRepository<CustomerEntity>
{
    Task<CustomerEntity?> GetByEmailAsync(string email);
}
