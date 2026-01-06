using System.ComponentModel.DataAnnotations;

namespace IMS_API.dtos.UserDTO;

public class UserUpdateDto
{
    [MaxLength(200)]
    public string? FullName { get; set; }

    public string? Role { get; set; } // admin, owner, manager
}
