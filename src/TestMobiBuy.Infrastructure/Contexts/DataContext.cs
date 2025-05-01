using Microsoft.EntityFrameworkCore;
using TestMobiBuy.Domain.Entities;
using TestMobiBuy.Infrastructure.Mappings;

namespace TestMobiBuy.Infrastructure.Contexts;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<CustomerAddressEntity> CustomerAddresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerEntityMap());
        modelBuilder.ApplyConfiguration(new CustomerAddressMap());
    }
}
