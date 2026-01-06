using IMS_API.dtos.InventoryItemDTO;

namespace IMS_API.Services.Interfaces;

public interface IInventoryItemService
{
    Task<InventoryItemDto?> GetByIdAsync(int id);
    Task<List<InventoryItemDto>> GetAllAsync();
    Task<InventoryItemDto?> GetByProductAndWarehouseAsync(int productId, int warehouseId);
    Task<InventoryItemDto> CreateAsync(InventoryItemCreateDto dto);
    Task<InventoryItemDto?> UpdateAsync(int id, InventoryItemUpdateDto dto);

    Task<InventoryItemDto?> UpdateByProductIdAsync(int productId, InventoryItemUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
