using IMS_API.Models;

namespace IMS_API.Repositories.Interfaces;

public interface IInventoryItemRepository
{
    Task<InventoryItem?> GetByIdAsync(int id);
    Task<List<InventoryItem>> GetAllAsync();
    Task<InventoryItem?> GetByProductAndWarehouseAsync(int productId, int warehouseId);
    Task<InventoryItem?> GetByProductIdAsync(int productId);
    Task<InventoryItem> CreateAsync(InventoryItem inventoryItem);
    Task<InventoryItem?> UpdateAsync(int id, InventoryItem inventoryItem);
    Task<InventoryItem?> UpdateByProductIdAsync(int productId, InventoryItem inventoryItem);
    Task<bool> DeleteAsync(int id);
}
