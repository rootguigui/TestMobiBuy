namespace TestMobiBuy.Application.Models.Response;

public class CustomerResponseModel
{
    public long CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IEnumerable<CustomerAddressResponseModel> Addresses { get; set; } = new List<CustomerAddressResponseModel>();
}

public class CustomerAddressResponseModel
{
    public long CustomerAddressId { get; set; }
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
}
