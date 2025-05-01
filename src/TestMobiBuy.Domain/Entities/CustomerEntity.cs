namespace TestMobiBuy.Domain.Entities;

public class CustomerEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public virtual ICollection<CustomerAddressEntity> Addresses { get; set; } = new List<CustomerAddressEntity>();
}
