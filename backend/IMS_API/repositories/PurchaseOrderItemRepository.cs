using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS_API.Repositories;

public class PurchaseOrderItemRepository : IPurchaseOrderItemRepository
{
    private readonly ApplicationDbContext _db;

    public PurchaseOrderItemRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<PurchaseOrderItem?> GetByIdAsync(int id)
    {
        return _db.PurchaseOrderItems.Include(p => p.Product).Include(p => p.Warehouse).SingleOrDefaultAsync(p => p.Id == id);
    }

    public Task<List<PurchaseOrderItem>> GetAllAsync()
    {
        return _db.PurchaseOrderItems.Include(p => p.Product).Include(p => p.Warehouse).ToListAsync();
    }

    public Task<List<PurchaseOrderItem>> GetByPurchaseOrderIdAsync(int purchaseOrderId)
    {
        return _db.PurchaseOrderItems
            .Include(p => p.Product)
            .Include(p => p.Warehouse)
            .Where(p => p.PurchaseOrderId == purchaseOrderId)
            .ToListAsync();
    }

    public async Task<PurchaseOrderItem> CreateAsync(PurchaseOrderItem purchaseOrderItem)
    {
        _db.PurchaseOrderItems.Add(purchaseOrderItem);
        await _db.SaveChangesAsync();
        return purchaseOrderItem;
    }

    public async Task<PurchaseOrderItem?> UpdateAsync(int id, PurchaseOrderItem purchaseOrderItem)
    {
        var existingItem = await _db.PurchaseOrderItems.SingleOrDefaultAsync(p => p.Id == id);
        if (existingItem == null) return null;

        existingItem.Quantity = purchaseOrderItem.Quantity;
        existingItem.UnitPrice = purchaseOrderItem.UnitPrice;
        existingItem.WarehouseId = purchaseOrderItem.WarehouseId;

        await _db.SaveChangesAsync();
        return existingItem;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _db.PurchaseOrderItems.SingleOrDefaultAsync(p => p.Id == id);
        if (item == null) return false;

        _db.PurchaseOrderItems.Remove(item);
        await _db.SaveChangesAsync();
        return true;
    }
}
