using System.ComponentModel.DataAnnotations;

namespace IMS_API.dtos.ProductDTO;

public class ProductCreateDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = "";
    [MaxLength(50)]
    public string SKU { get; set; } = "";

    public int? CategoryId { get; set; }

    [Required]
    [Range(0, 9999999.99)]
    public decimal Price { get; set; }
    [Required]
    public int? UserId { get; set; }
    [MaxLength(1000)]
    public string Description { get; set; } = "";

    public bool IsActive { get; set; }
}
