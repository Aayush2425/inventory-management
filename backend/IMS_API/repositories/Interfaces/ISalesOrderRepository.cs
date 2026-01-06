using IMS_API.Models;

namespace IMS_API.Repositories.Interfaces;

public interface ISalesOrderRepository
{
    Task<SalesOrder?> GetByIdAsync(int id);
    Task<List<SalesOrder>> GetAllAsync();
    Task<List<SalesOrder>> GetByCustomerIdAsync(int customerId);
    Task<SalesOrder> CreateAsync(SalesOrder salesOrder);
    Task<SalesOrder?> UpdateAsync(int id, SalesOrder salesOrder);
    Task<bool> DeleteAsync(int id);
}
