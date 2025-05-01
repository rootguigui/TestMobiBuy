using TestMobiBuy.Domain.Entities;
using TestMobiBuy.Infrastructure.Contexts;
using TestMobiBuy.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TestMobiBuy.Infrastructure.Repositories;

public class CustomerRepository : BaseRepository<CustomerEntity>, ICustomerRepository
{
    public CustomerRepository(DataContext context) : base(context)
    {
    }

    public async Task<CustomerEntity?> GetByEmailAsync(string email)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
    }
}
