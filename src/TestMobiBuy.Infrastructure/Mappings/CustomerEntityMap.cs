using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestMobiBuy.Domain.Entities;

namespace TestMobiBuy.Infrastructure.Mappings;

public class CustomerEntityMap : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.ToTable("customers", "app");
        builder.Property(c => c.Id).HasColumnName("customer_id").UseIdentityAlwaysColumn().HasColumnType("integer").IsRequired();
        builder.Property(c => c.Name).HasColumnName("name").HasColumnType("varchar").HasMaxLength(100).IsRequired();
        builder.Property(c => c.Email).HasColumnName("email").HasColumnType("varchar").HasMaxLength(100).IsRequired();
        builder.Property(c => c.IsActive).HasColumnName("is_active").HasColumnType("boolean").IsRequired();
        builder.Property(c => c.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp").IsRequired();
        builder.Property(c => c.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp");
        builder.Property(c => c.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp");

        builder.HasMany(c => c.Addresses).WithOne(a => a.Customer).HasForeignKey(a => a.CustomerId);
    }
}
