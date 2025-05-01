namespace TestMobiBuy.Application.Dtos;

public class CustomerAddressDto
{
    public long CustomerAddressId { get; set; }
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
}
