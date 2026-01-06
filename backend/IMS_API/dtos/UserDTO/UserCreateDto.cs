using System.ComponentModel.DataAnnotations;

namespace IMS_API.dtos.UserDTO;

public class UserCreateDto
{
    [Required, MaxLength(120)]
    public string Username { get; set; } = null!;

    [MaxLength(200)]
    public string? FullName { get; set; }

    [Required, MinLength(6)]
    public string Password { get; set; } = null!;

    [Required]
    public string Role { get; set; } = null!; // admin, owner, manager
}
