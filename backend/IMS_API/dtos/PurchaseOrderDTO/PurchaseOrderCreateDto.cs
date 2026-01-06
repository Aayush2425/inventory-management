using System.ComponentModel.DataAnnotations;
using IMS_API.dtos.PurchaseOrderItemDTO;
using Microsoft.EntityFrameworkCore;

namespace IMS_API.dtos.PurchaseOrderDTO;

public class PurchaseOrderCreateDto
{
    [Required, MaxLength(30)]
    public string OrderNumber { get; set; } = null!;

    [Required]
    public int SupplierId { get; set; }

    [Required]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public DateTime? ExpectedDeliveryDate { get; set; }

    public bool IsReceived { get; set; } = false;

    [Precision(18, 2)]
    public decimal TotalAmount { get; set; } = 0;

}