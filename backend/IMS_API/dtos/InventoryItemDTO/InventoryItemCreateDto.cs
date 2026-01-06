using System.ComponentModel.DataAnnotations;

namespace IMS_API.dtos.InventoryItemDTO;

public class InventoryItemCreateDto
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    public int WarehouseId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public int ReorderLevel { get; set; }

    public int Reserved { get; set; } = 0;
}
