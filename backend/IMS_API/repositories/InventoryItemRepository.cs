using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS_API.Repositories;

public class InventoryItemRepository : IInventoryItemRepository
{
    private readonly ApplicationDbContext _db;

    public InventoryItemRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<InventoryItem?> GetByIdAsync(int id)
    {
        return _db.InventoryItems.Include(i => i.Product).Include(i => i.Warehouse).SingleOrDefaultAsync(i => i.Id == id);
    }

    public Task<List<InventoryItem>> GetAllAsync()
    {
        return _db.InventoryItems.Include(i => i.Product).Include(i => i.Warehouse).ToListAsync();
    }

    public Task<InventoryItem?> GetByProductAndWarehouseAsync(int productId, int warehouseId)
    {
        return _db.InventoryItems
            .Include(i => i.Product)
            .Include(i => i.Warehouse)
            .SingleOrDefaultAsync(i => i.ProductId == productId && i.WarehouseId == warehouseId);
    }
    public Task<InventoryItem?> GetByProductIdAsync(int productId)
    {
        return _db.InventoryItems
            .Include(i => i.Product)
            .Include(i => i.Warehouse)
            .SingleOrDefaultAsync(i => i.ProductId == productId);
    }
    public async Task<InventoryItem> CreateAsync(InventoryItem inventoryItem)
    {
        _db.InventoryItems.Add(inventoryItem);
        await _db.SaveChangesAsync();
        return inventoryItem;
    }

    public async Task<InventoryItem?> UpdateAsync(int id, InventoryItem inventoryItem)
    {
        var existingItem = await _db.InventoryItems.SingleOrDefaultAsync(i => i.Id == id);
        if (existingItem == null) return null;

        existingItem.Quantity = inventoryItem.Quantity;
        existingItem.ReorderLevel = inventoryItem.ReorderLevel;
        existingItem.Reserved = inventoryItem.Reserved;
        existingItem.LastStockUpdateAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return existingItem;
    }
    public async Task<InventoryItem?> UpdateByProductIdAsync(int productId, InventoryItem inventoryItem)
    {
        var existingItem = await _db.InventoryItems.SingleOrDefaultAsync(i => i.ProductId == productId);
        if (existingItem == null) return null;

        existingItem.Quantity = inventoryItem.Quantity;
        existingItem.ReorderLevel = inventoryItem.ReorderLevel;
        existingItem.Reserved = inventoryItem.Reserved;
        existingItem.LastStockUpdateAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return existingItem;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _db.InventoryItems.SingleOrDefaultAsync(i => i.Id == id);
        if (item == null) return false;

        _db.InventoryItems.Remove(item);
        await _db.SaveChangesAsync();
        return true;
    }
}
