using Microsoft.EntityFrameworkCore;
namespace IMS_API.Models;

public class PurchaseOrderItem
{
    public int Id { get; set; }
    public int PurchaseOrderId { get; set; }
    public PurchaseOrder PurchaseOrder { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    [Precision(18, 2)]
    public decimal UnitPrice { get; set; }
    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;

    public string Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
