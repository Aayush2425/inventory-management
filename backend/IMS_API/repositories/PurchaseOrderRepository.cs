using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS_API.Repositories;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly ApplicationDbContext _db;

    public PurchaseOrderRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<PurchaseOrder?> GetByIdAsync(int id)
    {
        return _db.PurchaseOrders.Include(p => p.Supplier).Include(p => p.Items).SingleOrDefaultAsync(p => p.Id == id);
    }

    public Task<List<PurchaseOrder>> GetAllAsync()
    {
        return _db.PurchaseOrders.Include(p => p.Supplier).Include(p => p.Items).ToListAsync();
    }

    public Task<List<PurchaseOrder>> GetBySuppliersIdAsync(int supplierId)
    {
        return _db.PurchaseOrders.Include(p => p.Supplier).Include(p => p.Items).Where(p => p.SupplierId == supplierId).ToListAsync();
    }

    public Task<List<PurchaseOrder>> GetByWarehouseIdAsync(int warehouseId)
    {
        return _db.PurchaseOrders.Include(po => po.Supplier).Include(po => po.Items).ThenInclude(item => item.Product)
        .Where(po => po.Items.Any(item => item.WarehouseId == warehouseId))
        .ToListAsync();
    }
    public async Task<PurchaseOrder> CreateAsync(PurchaseOrder purchaseOrder)
    {
        _db.PurchaseOrders.Add(purchaseOrder);
        await _db.SaveChangesAsync();
        return purchaseOrder;
    }

    public async Task<PurchaseOrder?> UpdateAsync(int id, PurchaseOrder purchaseOrder)
    {
        var existingOrder = await _db.PurchaseOrders.SingleOrDefaultAsync(p => p.Id == id);
        if (existingOrder == null) return null;

        existingOrder.OrderNumber = purchaseOrder.OrderNumber ?? existingOrder.OrderNumber;
        existingOrder.SupplierId = purchaseOrder.SupplierId;
        existingOrder.IsReceived = purchaseOrder.IsReceived;
        existingOrder.ExpectedDeliveryDate = purchaseOrder.ExpectedDeliveryDate;
        existingOrder.OrderDate = purchaseOrder.OrderDate;
        existingOrder.TotalAmount = purchaseOrder.TotalAmount;
        existingOrder.UpdatedAt = purchaseOrder.UpdatedAt;


        await _db.SaveChangesAsync();
        return existingOrder;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var order = await _db.PurchaseOrders.SingleOrDefaultAsync(p => p.Id == id);
        if (order == null) return false;

        _db.PurchaseOrders.Remove(order);
        await _db.SaveChangesAsync();
        return true;
    }
}
