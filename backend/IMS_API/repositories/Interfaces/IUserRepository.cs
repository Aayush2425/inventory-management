using IMS_API.Models;

namespace IMS_API.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByNameAsync(string username);
    Task<User?> GetByIdAsync(int id);
    Task<List<User>> GetAllAsync();
    Task<User> CreateAsync(User user);
    Task<User?> UpdateAsync(int id, User user);
    Task<bool> DeleteAsync(int id);
}