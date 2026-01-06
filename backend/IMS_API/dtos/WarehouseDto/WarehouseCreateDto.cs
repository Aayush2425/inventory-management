using System.ComponentModel.DataAnnotations;
namespace IMS_API.dtos.WrehouseDTO;

public class WarehouseCreateDto
{
    [Required, MaxLength(120)]
    public string Name { get; set; } = null!;
    public string? Location { get; set; }
    public int UserId { get; set; }
}
