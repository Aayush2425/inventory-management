using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace IMS_API.Models;

public class PurchaseOrder
{
    public int Id { get; set; }
    [Required, MaxLength(30)]
    public string OrderNumber { get; set; } = null!;
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExpectedDeliveryDate { get; set; }
    public bool IsReceived { get; set; } = false;

    [Precision(18, 2)]
    public decimal TotalAmount { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
}
