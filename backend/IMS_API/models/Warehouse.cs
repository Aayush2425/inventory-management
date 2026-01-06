using System.ComponentModel.DataAnnotations;
namespace IMS_API.Models;

public class Warehouse
{
    public int Id { get; set; }
    [Required, MaxLength(120)]
    public string Name { get; set; } = null!;
    public string? Location { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
    public ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
