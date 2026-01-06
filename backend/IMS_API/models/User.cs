using System.ComponentModel.DataAnnotations;

namespace IMS_API.Models;

public class User
{
    public int Id { get; set; }
    [Required, MaxLength(120)]
    public string Username { get; set; } = null!;
    [MaxLength(200)]
    public string? FullName { get; set; }

    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    public string? RefreshToken { get; set; }

    public string Role { get; set; } = "";
    public DateTime? RefreshTokenExpiry { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Warehouse>? Warehouses { get; set; }
    public ICollection<Product>? CreatedProducts { get; set; } = new List<Product>();
}
