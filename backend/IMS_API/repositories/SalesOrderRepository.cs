using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS_API.Repositories;

public class SalesOrderRepository : ISalesOrderRepository
{
    private readonly ApplicationDbContext _db;

    public SalesOrderRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<SalesOrder?> GetByIdAsync(int id)
    {
        return _db.SalesOrders.Include(s => s.Customer).Include(s => s.Items).SingleOrDefaultAsync(s => s.Id == id);
    }

    public Task<List<SalesOrder>> GetAllAsync()
    {
        return _db.SalesOrders.Include(s => s.Customer).Include(s => s.Items).ToListAsync();
    }

    public Task<List<SalesOrder>> GetByCustomerIdAsync(int customerId)
    {
        return _db.SalesOrders.Include(s => s.Customer).Include(s => s.Items).Where(s => s.CustomerId == customerId).ToListAsync();
    }

    public async Task<SalesOrder> CreateAsync(SalesOrder salesOrder)
    {
        _db.SalesOrders.Add(salesOrder);
        await _db.SaveChangesAsync();
        return salesOrder;
    }

    public async Task<SalesOrder?> UpdateAsync(int id, SalesOrder salesOrder)
    {
        var existingOrder = await _db.SalesOrders.SingleOrDefaultAsync(s => s.Id == id);
        if (existingOrder == null) return null;

        existingOrder.OrderNumber = salesOrder.OrderNumber ?? existingOrder.OrderNumber;
        existingOrder.CustomerId = salesOrder.CustomerId;
        existingOrder.IsShipped = salesOrder.IsShipped;
        existingOrder.Discount = salesOrder.Discount;
        existingOrder.Tax = salesOrder.Tax;
        existingOrder.TotalAmount = salesOrder.TotalAmount;

        await _db.SaveChangesAsync();
        return existingOrder;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var order = await _db.SalesOrders.SingleOrDefaultAsync(s => s.Id == id);
        if (order == null) return false;

        _db.SalesOrders.Remove(order);
        await _db.SaveChangesAsync();
        return true;
    }
}
