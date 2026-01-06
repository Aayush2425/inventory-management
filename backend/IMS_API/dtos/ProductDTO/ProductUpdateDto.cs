using System.ComponentModel.DataAnnotations;

public class ProductUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(50)]
    public string? SKU { get; set; }

    public int? CategoryId { get; set; }

    [Required]
    [Range(0, 9999999999999999.99)]
    public decimal Price { get; set; }

    [Required]
    public int? UserId { get; set; }
    [MaxLength(2000)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

}
