using System.ComponentModel.DataAnnotations;

namespace IMS_API.dtos.InventoryItemDTO;

public class InventoryItemUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public int ReorderLevel { get; set; }

    public int Reserved { get; set; }
}
