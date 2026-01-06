using System.ComponentModel.DataAnnotations;
namespace IMS_API.dtos.AuthDTO;

public class RegisterDto
{
    [Required, MaxLength(120)]
    public string Username { get; set; } = null!;

    [Required, MinLength(6)]
    public string Password { get; set; } = null!;

    [MaxLength(200)]
    public string? FullName { get; set; }
    public string? Role { get; set; }
}
