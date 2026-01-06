using System.ComponentModel.DataAnnotations;

namespace IMS_API.dtos.SalesOrderDTO;

public class SalesOrderUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required, MaxLength(30)]
    public string OrderNumber { get; set; } = null!;

    [Required]
    public int CustomerId { get; set; }

    public bool IsShipped { get; set; }

    public decimal Discount { get; set; } = 0;

    public decimal Tax { get; set; } = 0;

    public decimal TotalAmount { get; set; } = 0;
}
