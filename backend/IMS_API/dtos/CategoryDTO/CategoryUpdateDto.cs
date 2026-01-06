using System.ComponentModel.DataAnnotations;
namespace IMS_API.dtos.CategoryDTO;

public class CategoryUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required, MaxLength(80)]
    public string Name { get; set; } = null!;
    [MaxLength(500)]
    public string? Description { get; set; }
}
