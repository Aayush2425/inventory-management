using System.ComponentModel.DataAnnotations;
namespace IMS_API.Models;

public class Category
{
    public int Id { get; set; }
    [Required, MaxLength(80)]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
