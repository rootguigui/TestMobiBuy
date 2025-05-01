using TestMobiBuy.Domain.Entities;

namespace TestMobiBuy.Domain.Interfaces;

public interface ICustomerAddressRepository : IBaseRepository<CustomerAddressEntity>
{
    Task<IEnumerable<CustomerAddressEntity>> GetByCustomerIdAsync(int customerId);
}
