using IMS_API.Models;

namespace IMS_API.Repositories.Interfaces;

public interface IWarehouseRepository
{
    Task<Warehouse?> GetByIdAsync(int id);
    Task<List<Warehouse>> GetAllAsync();
    Task<List<Warehouse>> GetByUserIdAsync(int userId);
    Task<Warehouse> CreateAsync(Warehouse warehouse);
    Task<Warehouse?> UpdateAsync(int id, Warehouse warehouse);
    Task<bool> DeleteAsync(int id);
}
