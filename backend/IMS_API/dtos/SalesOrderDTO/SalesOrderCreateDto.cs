using System.ComponentModel.DataAnnotations;

namespace IMS_API.dtos.SalesOrderDTO;

public class SalesOrderCreateDto
{
    [Required, MaxLength(30)]
    public string OrderNumber { get; set; } = null!;

    [Required]
    public int CustomerId { get; set; }

    public bool IsShipped { get; set; } = false;

    public decimal Discount { get; set; } = 0;

    public decimal Tax { get; set; } = 0;

    public decimal TotalAmount { get; set; } = 0;
}
