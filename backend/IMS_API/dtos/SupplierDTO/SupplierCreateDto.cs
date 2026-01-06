using System.ComponentModel.DataAnnotations;

namespace IMS_API.dtos.SupplierDTO;

public class SupplierCreateDto
{
    [Required, MaxLength(120)]
    public string Name { get; set; } = null!;

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    [Phone]
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(200)]
    public string? Address { get; set; }

    [Required]
    public int WarehouseId { get; set; }
}
