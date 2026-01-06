namespace IMS_API.dtos.CustomerDTO;

public class CustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public int WarehouseId { get; set; }
}
