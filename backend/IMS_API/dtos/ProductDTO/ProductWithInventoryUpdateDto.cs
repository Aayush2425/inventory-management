using IMS_API.dtos.InventoryItemDTO;

namespace IMS_API.dtos.ProductDTO;

public class ProductWithInventoryUpdateDto
{
    public ProductUpdateDto Product { get; set; } = null!;
    public InventoryItemUpdateDto Inventory { get; set; } = null!;
}