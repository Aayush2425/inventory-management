using IMS_API.Models;

namespace IMS_API.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id);
    Task<List<Category>> GetAllAsync();
    Task<Category> CreateAsync(Category Category);
    Task<Category?> UpdateAsync(int id, Category Category);
    Task<bool> DeleteAsync(int id);
}