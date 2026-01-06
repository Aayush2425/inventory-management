namespace IMS_API.dtos.SalesOrderItemDTO;

public class SalesOrderItemDto
{
    public int Id { get; set; }
    public int SalesOrderId { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int WarehouseId { get; set; }
    public string? WarehouseName { get; set; }
}
