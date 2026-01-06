namespace IMS_API.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

public class Product
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(50)]
    public string? SKU { get; set; } //Stock Keeping Unit unique identity

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    [Precision(18, 2)]
    public decimal Price { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }

    public ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
}
