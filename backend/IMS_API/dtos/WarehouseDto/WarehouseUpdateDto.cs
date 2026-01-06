using System.ComponentModel.DataAnnotations;

public class WarehouseUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Name { get; set; } = null!;
    public string? Location { get; set; }
}
