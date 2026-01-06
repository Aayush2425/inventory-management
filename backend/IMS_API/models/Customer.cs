using System.ComponentModel.DataAnnotations;

namespace IMS_API.Models;

public class Customer
{
    public int Id { get; set; }
    [Required, MaxLength(120)]
    public string Name { get; set; } = null!;

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    [Phone]
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(200)]
    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;
    public ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();
}