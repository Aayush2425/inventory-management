using IMS_API.Models;

namespace IMS_API.Repositories.Interfaces;

public interface ISupplierRepository
{
    Task<Supplier?> GetByIdAsync(int id);
    Task<List<Supplier>> GetAllAsync();
    Task<List<Supplier>> GetByWarehouseIdAsync(int warehouseId);
    Task<Supplier> CreateAsync(Supplier supplier);
    Task<Supplier?> UpdateAsync(int id, Supplier supplier);
    Task<bool> DeleteAsync(int id);

    Task<List<Supplier>> SearchAsync(int userId, string? query, string? Warehouse);
}
