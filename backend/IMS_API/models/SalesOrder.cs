using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace IMS_API.Models;

public class SalesOrder
{
    public int Id { get; set; }
    [Required, MaxLength(30)]
    public string OrderNumber { get; set; } = null!;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public bool IsShipped { get; set; } = false;

    [Precision(18, 2)]
    public decimal Discount { get; set; } = 0;

    [Precision(18, 2)]
    public decimal Tax { get; set; } = 0;

    [Precision(18, 2)]
    public decimal TotalAmount { get; set; } = 0;

    public bool PaymentStatus { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<SalesOrderItem> Items { get; set; } = new List<SalesOrderItem>();
}
