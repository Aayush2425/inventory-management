using System.ComponentModel.DataAnnotations;
using IMS_API.Models;
using Microsoft.EntityFrameworkCore;

namespace IMS_API.dtos.PurchaseOrderItemDTO;

public class PurchaseOrderItemCreateDto
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    public int PurchaseOrderId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Precision(18, 2)]
    public decimal UnitPrice { get; set; }

    [Required]
    public int WarehouseId { get; set; }

    public string Status { get; set; }
}
