using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS_API.Repositories;

public class SalesOrderItemRepository : ISalesOrderItemRepository
{
    private readonly ApplicationDbContext _db;

    public SalesOrderItemRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<SalesOrderItem?> GetByIdAsync(int id)
    {
        return _db.SalesOrderItems.Include(s => s.Product).Include(s => s.Warehouse).SingleOrDefaultAsync(s => s.Id == id);
    }

    public Task<List<SalesOrderItem>> GetAllAsync()
    {
        return _db.SalesOrderItems.Include(s => s.Product).Include(s => s.Warehouse).ToListAsync();
    }

    public Task<List<SalesOrderItem>> GetBySalesOrderIdAsync(int salesOrderId)
    {
        return _db.SalesOrderItems
            .Include(s => s.Product)
            .Include(s => s.Warehouse)
            .Where(s => s.SalesOrderId == salesOrderId)
            .ToListAsync();
    }

    public async Task<SalesOrderItem> CreateAsync(SalesOrderItem salesOrderItem)
    {
        _db.SalesOrderItems.Add(salesOrderItem);
        await _db.SaveChangesAsync();
        return salesOrderItem;
    }

    public async Task<SalesOrderItem?> UpdateAsync(int id, SalesOrderItem salesOrderItem)
    {
        var existingItem = await _db.SalesOrderItems.SingleOrDefaultAsync(s => s.Id == id);
        if (existingItem == null) return null;

        existingItem.Quantity = salesOrderItem.Quantity;
        existingItem.UnitPrice = salesOrderItem.UnitPrice;
        existingItem.WarehouseId = salesOrderItem.WarehouseId;

        await _db.SaveChangesAsync();
        return existingItem;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _db.SalesOrderItems.SingleOrDefaultAsync(s => s.Id == id);
        if (item == null) return false;

        _db.SalesOrderItems.Remove(item);
        await _db.SaveChangesAsync();
        return true;
    }
}
