using System.Data;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace IMS_API.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _configuration;

    public CustomerRepository(ApplicationDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public Task<Customer?> GetByIdAsync(int id)
    {
        return _db.Customers.SingleOrDefaultAsync(c => c.Id == id);
    }

    public Task<List<Customer>> GetAllAsync()
    {
        return _db.Customers.ToListAsync();
    }

    public Task<List<Customer>> GetByWarehouseIdAsync(int warehouseId)
    {
        return _db.Customers
            .Where(c => c.WarehouseId == warehouseId)
            .ToListAsync();
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer?> UpdateAsync(int id, Customer customer)
    {
        var existingCustomer = await _db.Customers.SingleOrDefaultAsync(c => c.Id == id);
        if (existingCustomer == null) return null;

        existingCustomer.Name = customer.Name ?? existingCustomer.Name;
        existingCustomer.Email = customer.Email ?? existingCustomer.Email;
        existingCustomer.PhoneNumber = customer.PhoneNumber ?? existingCustomer.PhoneNumber;
        existingCustomer.Address = customer.Address ?? existingCustomer.Address;

        await _db.SaveChangesAsync();
        return existingCustomer;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _db.Customers.SingleOrDefaultAsync(c => c.Id == id);
        if (customer == null) return false;

        _db.Customers.Remove(customer);
        await _db.SaveChangesAsync();
        return true;
    }

    public Task<List<Customer>> SearchAsync(int userId, string? query, string? warehouse)
    {
        var customers = new List<Customer>();
        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            using (var command = new SqlCommand("Customer_Search_By_Name_Warehouse", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Query", (object?)query ?? DBNull.Value);
                command.Parameters.AddWithValue("@Warehouse", (object?)warehouse ?? DBNull.Value);

                connection.Open();
                var reader = command.ExecuteReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);

                customers = dataTable.AsEnumerable()
                    .Select(row => new Customer
                    {
                        Id = row.Field<int>("Id"),
                        Name = row.Field<string>("Name")!,
                        Email = row.Field<string>("Email"),
                        PhoneNumber = row.Field<string>("PhoneNumber"),
                        Address = row.Field<string>("Address"),
                        WarehouseId = row.Field<int>("WarehouseId"),
                    })
                    .ToList();
            }
        }
        return Task.FromResult(customers);

    }

}
