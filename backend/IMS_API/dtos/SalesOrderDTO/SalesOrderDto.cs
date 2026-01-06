using IMS_API.dtos.SalesOrderItemDTO;

namespace IMS_API.dtos.SalesOrderDTO;

public class SalesOrderDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public bool IsShipped { get; set; }
    public decimal Discount { get; set; } = 0;
    public decimal Tax { get; set; } = 0;
    public decimal TotalAmount { get; set; } = 0;
    public List<SalesOrderItemDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
