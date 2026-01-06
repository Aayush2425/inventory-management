using IMS_API.Models;

namespace IMS_API.Repositories.Interfaces;

public interface IPurchaseOrderItemRepository
{
    Task<PurchaseOrderItem?> GetByIdAsync(int id);
    Task<List<PurchaseOrderItem>> GetAllAsync();
    Task<List<PurchaseOrderItem>> GetByPurchaseOrderIdAsync(int purchaseOrderId);
    Task<PurchaseOrderItem> CreateAsync(PurchaseOrderItem purchaseOrderItem);
    Task<PurchaseOrderItem?> UpdateAsync(int id, PurchaseOrderItem purchaseOrderItem);
    Task<bool> DeleteAsync(int id);
}
