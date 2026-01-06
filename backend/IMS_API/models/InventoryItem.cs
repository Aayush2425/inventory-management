
namespace IMS_API.Models;

public class InventoryItem
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;

    public int Quantity { get; set; } = 0;

    public int ReorderLevel { get; set; } = 0;

    public int ReorderQuantity { get; set; } = 0;

    public int Reserved { get; set; } = 0;

    public DateTime LastStockUpdateAt { get; set; } = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
