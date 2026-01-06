using System.ComponentModel.DataAnnotations;
using IMS_API.dtos.SalesOrderItemDTO;

namespace IMS_API.dtos.SalesOrderDTO;

public class SalesWithItemsCreate
{
    [Required]
    public SalesOrderCreateDto SalesOrder { get; set; } = new();

    [Required]
    public List<SalesOrderItemCreateDto> Items { get; set; } = new();
}

public class SalesWithItemsUpdate
{
    [Required]
    public SalesOrderUpdateDto SalesOrder { get; set; } = new();

    [Required]
    public List<SalesOrderItemCreateDto> Items { get; set; } = new();
}
