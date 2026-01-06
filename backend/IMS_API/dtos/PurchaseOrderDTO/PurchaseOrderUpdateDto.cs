using System.ComponentModel.DataAnnotations;
using IMS_API.dtos.PurchaseOrderItemDTO;
using Microsoft.EntityFrameworkCore;

namespace IMS_API.dtos.PurchaseOrderDTO;

public class PurchaseOrderUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required, MaxLength(30)]
    public string OrderNumber { get; set; } = null!;

    [Required]
    public int SupplierId { get; set; }

    public DateTime OrderDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }

    public bool IsReceived { get; set; }

    [Precision(18, 2)]
    public decimal TotalAmount { get; set; }

    // 👇 Optional if editing items separately
    public List<PurchaseOrderItemUpdateDto>? Items { get; set; }
}
