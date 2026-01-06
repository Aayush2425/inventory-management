using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS_API.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly ApplicationDbContext _db;

    public WarehouseRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<Warehouse?> GetByIdAsync(int id)
    {
        return _db.Warehouses.Include(w => w.User).SingleOrDefaultAsync(w => w.Id == id);
    }

    public Task<List<Warehouse>> GetAllAsync()
    {
        return _db.Warehouses.Include(w => w.User).ToListAsync();
    }

    public Task<List<Warehouse>> GetByUserIdAsync(int userId)
    {
        return _db.Warehouses.Include(w => w.User).Where(w => w.UserId == userId).ToListAsync();
    }

    public async Task<Warehouse> CreateAsync(Warehouse warehouse)
    {
        _db.Warehouses.Add(warehouse);
        await _db.SaveChangesAsync();
        return warehouse;
    }

    public async Task<Warehouse?> UpdateAsync(int id, Warehouse warehouse)
    {
        var existingWarehouse = await _db.Warehouses.SingleOrDefaultAsync(w => w.Id == id);
        if (existingWarehouse == null) return null;

        existingWarehouse.Name = warehouse.Name ?? existingWarehouse.Name;
        existingWarehouse.Location = warehouse.Location ?? existingWarehouse.Location;

        await _db.SaveChangesAsync();
        return existingWarehouse;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var warehouse = await _db.Warehouses.SingleOrDefaultAsync(w => w.Id == id);
        if (warehouse == null) return false;

        _db.Warehouses.Remove(warehouse);
        await _db.SaveChangesAsync();
        return true;
    }
}
