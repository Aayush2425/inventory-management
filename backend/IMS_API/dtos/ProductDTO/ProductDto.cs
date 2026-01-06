using IMS_API.dtos.InventoryItemDTO;
using IMS_API.Models;

namespace IMS_API.dtos.ProductDTO;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? SKU { get; set; }
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public int? UserId { get; set; }
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    public List<InventoryItemDto>? InventoryItems { get; set; }
}
