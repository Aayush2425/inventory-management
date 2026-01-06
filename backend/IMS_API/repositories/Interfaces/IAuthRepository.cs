using IMS_API.Models;

namespace IMS_API.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByRefreshTokenAsync(int userId, string refreshToken);
    Task<User> CreateAsync(User user);
    Task SaveChangesAsync();
}