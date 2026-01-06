using IMS_API.dtos.PurchaseOrderItemDTO;

namespace IMS_API.Services.Interfaces;

public interface IPurchaseOrderItemService
{
    Task<PurchaseOrderItemDto?> GetByIdAsync(int id);
    Task<List<PurchaseOrderItemDto>> GetAllAsync();
    Task<List<PurchaseOrderItemDto>> GetByPurchaseOrderIdAsync(int purchaseOrderId);
    Task<PurchaseOrderItemDto> CreateAsync(PurchaseOrderItemCreateDto dto);
    Task<PurchaseOrderItemDto?> UpdateAsync(int id, PurchaseOrderItemUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
