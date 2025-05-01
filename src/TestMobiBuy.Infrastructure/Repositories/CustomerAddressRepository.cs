using TestMobiBuy.Domain.Entities;
using TestMobiBuy.Infrastructure.Contexts;
using TestMobiBuy.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TestMobiBuy.Infrastructure.Repositories;

public class CustomerAddressRepository : BaseRepository<CustomerAddressEntity>, ICustomerAddressRepository
{
    public CustomerAddressRepository(DataContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CustomerAddressEntity>> GetByCustomerIdAsync(int customerId)
    {
        return await _context.CustomerAddresses.Where(x => x.CustomerId == customerId).ToListAsync();
    }
}
