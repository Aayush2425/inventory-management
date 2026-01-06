using IMS_API.dtos.PurchaseOrderItemDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services.Interfaces;

namespace IMS_API.Services;

public class PurchaseOrderItemService : IPurchaseOrderItemService
{
    private readonly IPurchaseOrderItemRepository _purchaseOrderItemRepository;

    public PurchaseOrderItemService(IPurchaseOrderItemRepository purchaseOrderItemRepository)
    {
        _purchaseOrderItemRepository = purchaseOrderItemRepository;
    }

    public async Task<PurchaseOrderItemDto?> GetByIdAsync(int id)
    {
        var item = await _purchaseOrderItemRepository.GetByIdAsync(id);
        if (item == null) return null;
        return MapToResponseDto(item);
    }

    public async Task<List<PurchaseOrderItemDto>> GetAllAsync()
    {
        var items = await _purchaseOrderItemRepository.GetAllAsync();
        return items.Select(MapToResponseDto).ToList();
    }

    public async Task<List<PurchaseOrderItemDto>> GetByPurchaseOrderIdAsync(int purchaseOrderId)
    {
        var items = await _purchaseOrderItemRepository.GetByPurchaseOrderIdAsync(purchaseOrderId);
        return items.Select(MapToResponseDto).ToList();
    }

    public async Task<PurchaseOrderItemDto> CreateAsync(PurchaseOrderItemCreateDto dto)
    {
        var item = new PurchaseOrderItem
        {
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice,
            WarehouseId = dto.WarehouseId,
            PurchaseOrderId = dto.PurchaseOrderId,
            Status = dto.Status
        };

        var createdItem = await _purchaseOrderItemRepository.CreateAsync(item);
        return MapToResponseDto(createdItem);
    }

    public async Task<PurchaseOrderItemDto?> UpdateAsync(int id, PurchaseOrderItemUpdateDto dto)
    {
        var existingItem = await _purchaseOrderItemRepository.GetByIdAsync(id);
        if (existingItem == null) throw new InvalidOperationException("Purchase order item doesn't exist");

        var itemToUpdate = new PurchaseOrderItem
        {
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice,
            WarehouseId = dto.WarehouseId,
            Status = dto.Status,

        };

        var updatedItem = await _purchaseOrderItemRepository.UpdateAsync(id, itemToUpdate);
        return updatedItem != null ? MapToResponseDto(updatedItem) : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _purchaseOrderItemRepository.DeleteAsync(id);
    }

    private PurchaseOrderItemDto MapToResponseDto(PurchaseOrderItem item)
    {
        return new PurchaseOrderItemDto
        {
            Id = item.Id,
            PurchaseOrderId = item.PurchaseOrderId,
            ProductId = item.ProductId,
            ProductName = item.Product?.Name,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            WarehouseId = item.WarehouseId,
            WarehouseName = item.Warehouse?.Name
        };
    }
}
