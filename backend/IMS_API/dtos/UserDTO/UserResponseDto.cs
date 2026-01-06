namespace IMS_API.dtos.UserDTO;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string? FullName { get; set; }
    public string Role { get; set; } = "";
}
