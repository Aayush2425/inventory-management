using IMS_API.dtos.PurchaseOrderDTO;

namespace IMS_API.Services.Interfaces;

public interface IPurchaseOrderService
{
    Task<PurchaseOrderDto?> GetByIdAsync(int id);
    Task<List<PurchaseOrderDto>> GetAllAsync();
    Task<List<PurchaseOrderDto>> GetBySuppliersIdAsync(int supplierId);
    Task<List<PurchaseOrderDto>> GetByWarehouseIdAsync(int warehouseId);

    Task<PurchaseOrderDto> CreateAsync(PurchaseOrderCreateDto dto);
    Task<PurchaseOrderDto?> UpdateAsync(int id, PurchaseOrderUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
