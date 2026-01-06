using IMS_API.Models;

namespace IMS_API.Repositories.Interfaces;

public interface IPurchaseOrderRepository
{
    Task<PurchaseOrder?> GetByIdAsync(int id);
    Task<List<PurchaseOrder>> GetAllAsync();
    Task<List<PurchaseOrder>> GetBySuppliersIdAsync(int supplierId);
    Task<List<PurchaseOrder>> GetByWarehouseIdAsync(int warehouseId);
    Task<PurchaseOrder> CreateAsync(PurchaseOrder purchaseOrder);
    Task<PurchaseOrder?> UpdateAsync(int id, PurchaseOrder purchaseOrder);
    Task<bool> DeleteAsync(int id);
}
