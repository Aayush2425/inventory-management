using IMS_API.dtos.InventoryItemDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services.Interfaces;

namespace IMS_API.Services;

public class InventoryItemService : IInventoryItemService
{
    private readonly IInventoryItemRepository _inventoryItemRepository;

    public InventoryItemService(IInventoryItemRepository inventoryItemRepository)
    {
        _inventoryItemRepository = inventoryItemRepository;
    }

    public async Task<InventoryItemDto?> GetByIdAsync(int id)
    {
        var item = await _inventoryItemRepository.GetByIdAsync(id);
        if (item == null) return null;
        return MapToResponseDto(item);
    }

    public async Task<List<InventoryItemDto>> GetAllAsync()
    {
        var items = await _inventoryItemRepository.GetAllAsync();
        return items.Select(MapToResponseDto).ToList();
    }

    public async Task<InventoryItemDto?> GetByProductAndWarehouseAsync(int productId, int warehouseId)
    {
        var item = await _inventoryItemRepository.GetByProductAndWarehouseAsync(productId, warehouseId);
        if (item == null) return null;
        return MapToResponseDto(item);
    }

    public async Task<InventoryItemDto> CreateAsync(InventoryItemCreateDto dto)
    {
        var item = new InventoryItem
        {
            ProductId = dto.ProductId,
            WarehouseId = dto.WarehouseId,
            Quantity = dto.Quantity,
            ReorderLevel = dto.ReorderLevel,
            Reserved = dto.Reserved
        };

        var createdItem = await _inventoryItemRepository.CreateAsync(item);
        return MapToResponseDto(createdItem);
    }

    public async Task<InventoryItemDto?> UpdateAsync(int id, InventoryItemUpdateDto dto)
    {
        var existingItem = await _inventoryItemRepository.GetByIdAsync(id);
        if (existingItem == null) throw new InvalidOperationException("Inventory item doesn't exist");

        var itemToUpdate = new InventoryItem
        {
            Quantity = dto.Quantity,
            ReorderLevel = dto.ReorderLevel,
            Reserved = dto.Reserved
        };

        var updatedItem = await _inventoryItemRepository.UpdateAsync(id, itemToUpdate);
        return updatedItem != null ? MapToResponseDto(updatedItem) : null;
    }
    public async Task<InventoryItemDto?> UpdateByProductIdAsync(int productId, InventoryItemUpdateDto dto)
    {
        var existingItem = await _inventoryItemRepository.GetByProductIdAsync(productId);
        if (existingItem == null) throw new InvalidOperationException("Inventory item doesn't exist");

        var itemToUpdate = new InventoryItem
        {
            Quantity = dto.Quantity,
            ReorderLevel = dto.ReorderLevel,
        };

        var updatedItem = await _inventoryItemRepository.UpdateByProductIdAsync(productId, itemToUpdate);
        return updatedItem != null ? MapToResponseDto(updatedItem) : null;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        return await _inventoryItemRepository.DeleteAsync(id);
    }

    private InventoryItemDto MapToResponseDto(InventoryItem item)
    {
        return new InventoryItemDto
        {
            Id = item.Id,
            ProductId = item.ProductId,
            ProductName = item.Product?.Name,
            WarehouseId = item.WarehouseId,
            WarehouseName = item.Warehouse?.Name,
            Quantity = item.Quantity,
            ReorderLevel = item.ReorderLevel,
            Reserved = item.Reserved,
            LastStockUpdateAt = item.LastStockUpdateAt
        };
    }
}
