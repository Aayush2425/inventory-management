using System.Data;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace IMS_API.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _configuration;

    public SupplierRepository(ApplicationDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public Task<Supplier?> GetByIdAsync(int id)
    {
        return _db.Suppliers.SingleOrDefaultAsync(s => s.Id == id);
    }

    public Task<List<Supplier>> GetAllAsync()
    {
        return _db.Suppliers.ToListAsync();
    }
    public Task<List<Supplier>> GetByWarehouseIdAsync(int warehouseId)
    {
        return _db.Suppliers.Where(s => s.WarehouseId == warehouseId).ToListAsync();
    }
    public async Task<Supplier> CreateAsync(Supplier supplier)
    {
        _db.Suppliers.Add(supplier);
        await _db.SaveChangesAsync();
        return supplier;
    }

    public async Task<Supplier?> UpdateAsync(int id, Supplier supplier)
    {
        var existingSupplier = await _db.Suppliers.SingleOrDefaultAsync(s => s.Id == id);
        if (existingSupplier == null) return null;

        existingSupplier.Name = supplier.Name ?? existingSupplier.Name;
        existingSupplier.Email = supplier.Email ?? existingSupplier.Email;
        existingSupplier.PhoneNumber = supplier.PhoneNumber ?? existingSupplier.PhoneNumber;
        existingSupplier.Address = supplier.Address ?? existingSupplier.Address;

        await _db.SaveChangesAsync();
        return existingSupplier;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var supplier = await _db.Suppliers.SingleOrDefaultAsync(s => s.Id == id);
        if (supplier == null) return false;

        _db.Suppliers.Remove(supplier);
        await _db.SaveChangesAsync();
        return true;
    }

    public Task<List<Supplier>> SearchAsync(int userId, string? query, string? warehouse)
    {
        var suppliers = new List<Supplier>();
        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        using (var command = new SqlCommand("Supplier_Search_By_Name_And_Warehouse", connection))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@Query", query);
            command.Parameters.AddWithValue("@Warehouse", warehouse);

            connection.Open();

            using (var adapter = new SqlDataAdapter(command))
            {
                var table = new DataTable();
                adapter.Fill(table);

                suppliers = table.AsEnumerable()
                    .Select(row => new Supplier
                    {
                        Id = row.Field<int>("Id"),
                        Name = row.Field<string>("Name")!,
                        Email = row.Field<string>("Email")!,
                        PhoneNumber = row.Field<string>("PhoneNumber")!,
                        Address = row.Field<string>("Address")!,
                        WarehouseId = row.Field<int>("WarehouseId"),
                    })
                    .ToList();
            }
        }
        return Task.FromResult(suppliers);
    }

}
