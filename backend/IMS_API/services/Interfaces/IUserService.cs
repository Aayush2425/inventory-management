using IMS_API.dtos.UserDTO;
using IMS_API.Models;

namespace IMS_API.Services.Interfaces;

public interface IUserService
{
    Task<UserResponseDto?> GetByIdAsync(int id);
    Task<List<UserResponseDto>> GetAllAsync();
    Task<UserResponseDto> CreateAsync(UserCreateDto dto);
    Task<UserResponseDto?> UpdateAsync(int id, UserUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
