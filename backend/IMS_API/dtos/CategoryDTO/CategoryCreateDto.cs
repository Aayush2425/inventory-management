using System.ComponentModel.DataAnnotations;
namespace IMS_API.dtos.CategoryDTO;

public class CategoryCreateDto
{
    [Required, MaxLength(80)]
    public string Name { get; set; } = null!;
    [MaxLength(500)]
    public string? Description { get; set; }
}
