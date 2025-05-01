using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestMobiBuy.Domain.Entities;

namespace TestMobiBuy.Infrastructure.Mappings;

public class CustomerAddressMap : IEntityTypeConfiguration<CustomerAddressEntity>
{
    public void Configure(EntityTypeBuilder<CustomerAddressEntity> builder)
    {
        builder.ToTable("customer_addresses", "app");
        builder.Property(a => a.Id).HasColumnName("customer_address_id").UseIdentityAlwaysColumn().HasColumnType("integer").IsRequired();
        builder.Property(a => a.Address).HasColumnName("address").HasColumnType("varchar").HasMaxLength(100).IsRequired();
        builder.Property(a => a.City).HasColumnName("city").HasColumnType("varchar").HasMaxLength(100).IsRequired();
        builder.Property(a => a.State).HasColumnName("state").HasColumnType("varchar").HasMaxLength(100).IsRequired();
        builder.Property(a => a.ZipCode).HasColumnName("zip_code").HasColumnType("varchar").HasMaxLength(100).IsRequired();
        builder.Property(a => a.Country).HasColumnName("country").HasColumnType("varchar").HasMaxLength(100).IsRequired();
        builder.Property(a => a.Neighborhood).HasColumnName("neighborhood").HasColumnType("varchar").HasMaxLength(100).IsRequired();
        builder.Property(a => a.Number).HasColumnName("number").HasColumnType("varchar").HasMaxLength(100).IsRequired();
        builder.Property(a => a.Complement).HasColumnName("complement").HasColumnType("varchar").HasMaxLength(100).IsRequired();
        builder.Property(a => a.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp").IsRequired();
        builder.Property(a => a.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp");
        builder.Property(a => a.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp");
        builder.Property(a => a.CustomerId).HasColumnName("customer_id").HasColumnType("integer").IsRequired();
        builder.Property(a => a.IsActive).HasColumnName("is_active").HasColumnType("boolean").IsRequired();

        builder.HasOne(a => a.Customer).WithMany(c => c.Addresses).HasForeignKey(a => a.CustomerId);
    }
}

