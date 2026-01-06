using IMS_API.Models;

namespace IMS_API.Repositories.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<List<Product>> GetAllAsync();
    Task<List<Product>> GetByUserIdAsync(int userId);

    List<Product> GetByCategoryId(int categoryId);

    Task<Product> CreateAsync(Product Product);
    Task<Product?> UpdateAsync(int id, Product Product);
    Task<bool> DeleteAsync(int id);
    Task<List<Product>> SearchAsync(int userId, string? query, string? category);
}