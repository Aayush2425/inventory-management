using IMS_API.dtos.UserDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Helper;
using IMS_API.Services.Interfaces;

namespace IMS_API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, PasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserResponseDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) throw new InvalidOperationException("Id Mismatchd");

        return MapToResponseDto(user);
    }

    public async Task<List<UserResponseDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToResponseDto).ToList();
    }

    public async Task<UserResponseDto> CreateAsync(UserCreateDto dto)
    {
        var existingUser = await _userRepository.GetUserByNameAsync(dto.Username);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username already exists");
        }

        if (!IsValidRole(dto.Role))
        {
            throw new InvalidOperationException("Invalid role. Allowed roles: admin, owner, manager");
        }

        var (hash, salt) = _passwordHasher.HashPassword(dto.Password);
        var user = new User
        {
            Username = dto.Username,
            FullName = dto.FullName,
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = dto.Role
        };

        var createdUser = await _userRepository.CreateAsync(user);
        return MapToResponseDto(createdUser);
    }

    public async Task<UserResponseDto?> UpdateAsync(int id, UserUpdateDto dto)
    {
        if (dto.Role != null && !IsValidRole(dto.Role))
        {
            throw new InvalidOperationException("Invalid role. Allowed roles: admin, owner, manager");
        }

        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null) throw new InvalidOperationException("User doesn't exist");

        var userToUpdate = new User
        {
            FullName = dto.FullName,
            Role = dto.Role!
        };

        var updatedUser = await _userRepository.UpdateAsync(id, userToUpdate);
        return updatedUser != null ? MapToResponseDto(updatedUser) : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _userRepository.DeleteAsync(id);
    }

    private UserResponseDto MapToResponseDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            Role = user.Role
        };
    }

    private bool IsValidRole(string role)
    {
        var validRoles = new[] { "admin", "owner", "manager" };
        return validRoles.Contains(role.ToLower());
    }
}
