using IMS_API.Models;

namespace IMS_API.Repositories.Interfaces;

public interface ISalesOrderItemRepository
{
    Task<SalesOrderItem?> GetByIdAsync(int id);
    Task<List<SalesOrderItem>> GetAllAsync();
    Task<List<SalesOrderItem>> GetBySalesOrderIdAsync(int salesOrderId);
    Task<SalesOrderItem> CreateAsync(SalesOrderItem salesOrderItem);
    Task<SalesOrderItem?> UpdateAsync(int id, SalesOrderItem salesOrderItem);
    Task<bool> DeleteAsync(int id);
}
