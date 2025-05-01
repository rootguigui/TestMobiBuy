using System.ComponentModel.DataAnnotations;

namespace TestMobiBuy.Application.Models.Request;

public class CustomerCreateRequestModel
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [RegularExpression(@"^\d{5}-\d{3}$")]
    public string ZipCode { get; set; } = string.Empty;
}
