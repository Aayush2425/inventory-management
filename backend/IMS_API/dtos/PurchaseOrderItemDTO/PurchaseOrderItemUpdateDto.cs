using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace IMS_API.dtos.PurchaseOrderItemDTO;

public class PurchaseOrderItemUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Precision(18, 2)]
    public decimal UnitPrice { get; set; }

    [Required]
    public int WarehouseId { get; set; }

    public string Status { get; set; } = "Pending";
}