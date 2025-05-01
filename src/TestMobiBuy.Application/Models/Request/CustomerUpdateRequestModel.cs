namespace TestMobiBuy.Application.Models.Request;

public class CustomerUpdateRequestModel
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IEnumerable<CustomerAddressUpdateRequestModel> Addresses { get; set; } = new List<CustomerAddressUpdateRequestModel>();
}

public class CustomerAddressUpdateRequestModel
{
    public int CustomerAddressId { get; set; }
    public string ZipCode { get; set; } = string.Empty;
}
