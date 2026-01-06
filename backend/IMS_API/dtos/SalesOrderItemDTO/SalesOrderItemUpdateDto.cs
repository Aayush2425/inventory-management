using System.ComponentModel.DataAnnotations;

namespace IMS_API.dtos.SalesOrderItemDTO;

public class SalesOrderItemUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public int SalesOrderId { get; set; }
    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }

    [Required]
    public int WarehouseId { get; set; }
}
