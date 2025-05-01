namespace TestMobiBuy.Domain.Entities;

public class CustomerAddressEntity : BaseEntity
{
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Complement { get; set; } = string.Empty;

    public int CustomerId { get; set; }
    public virtual CustomerEntity Customer { get; set; } = default!;
}
