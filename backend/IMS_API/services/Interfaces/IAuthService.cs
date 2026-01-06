using IMS_API.dtos.AuthDTO;
using IMS_API.Models;

namespace IMS_API.Services.Interfaces;

public interface IAuthService
{
    Task<User> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}
