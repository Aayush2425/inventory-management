namespace IMS_API.dtos.WrehouseDTO;

public class WarehouseResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Location { get; set; }
    public int UserId { get; set; }
}
