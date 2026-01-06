namespace IMS_API.dtos.InventoryItemDTO;

public class InventoryItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public int WarehouseId { get; set; }
    public string? WarehouseName { get; set; }
    public int Quantity { get; set; }
    public int ReorderLevel { get; set; }
    public int Reserved { get; set; }
    public DateTime LastStockUpdateAt { get; set; }
}
