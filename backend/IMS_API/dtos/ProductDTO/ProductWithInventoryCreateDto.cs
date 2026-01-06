using System.ComponentModel.DataAnnotations;
using IMS_API.dtos.InventoryItemDTO;
using IMS_API.dtos.ProductDTO;
namespace IMS_API.dtos.ProductDTO;

public class ProductWithInventoryCreateDto
{
    [Required]
    public ProductCreateDto Product { get; set; } = new();

    [Required]
    public InventoryItemCreateDto Inventory { get; set; } = new();
}
