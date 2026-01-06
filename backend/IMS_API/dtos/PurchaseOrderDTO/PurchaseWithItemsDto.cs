using System.ComponentModel.DataAnnotations;
using IMS_API.dtos.PurchaseOrderItemDTO;

namespace IMS_API.dtos.PurchaseOrderDTO;

public class PurchaseWithItemsCreate
{
    [Required]
    public PurchaseOrderCreateDto PurchaseOrder { get; set; } = new();

    [Required]
    public List<PurchaseOrderItemCreateDto> Items { get; set; } = new();
}

public class PurchaseWithItemsUpdate
{
    [Required]
    public PurchaseOrderUpdateDto PurchaseOrder { get; set; } = new();

    [Required]
    public List<PurchaseOrderItemUpdateDto> Items { get; set; } = [];
}