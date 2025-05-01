namespace TestMobiBuy.Application.Dtos;

public class CustomerUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IEnumerable<CustomerAddressUpdateDto> Addresses { get; set; } = new List<CustomerAddressUpdateDto>();
}

public class CustomerAddressUpdateDto
{
    public int CustomerAddressId { get; set; }
    public string ZipCode { get; set; } = string.Empty;
}