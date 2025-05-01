namespace TestMobiBuy.Application.Dtos;

public class CustomerDto
{
    public long CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IEnumerable<CustomerAddressDto> Addresses { get; set; } = new List<CustomerAddressDto>();
}
