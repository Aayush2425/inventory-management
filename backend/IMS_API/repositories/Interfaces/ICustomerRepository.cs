using IMS_API.Models;

namespace IMS_API.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id);
    Task<List<Customer>> GetAllAsync();
    Task<Customer> CreateAsync(Customer customer);
    Task<List<Customer>> GetByWarehouseIdAsync(int warehouseId);

    Task<Customer?> UpdateAsync(int id, Customer customer);
    Task<bool> DeleteAsync(int id);

    Task<List<Customer>> SearchAsync(int userId, string? query, string? Warehouse);
}
