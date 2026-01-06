using IMS_API.dtos.SalesOrderItemDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services.Interfaces;

namespace IMS_API.Services;

public class SalesOrderItemService : ISalesOrderItemService
{
    private readonly ISalesOrderItemRepository _salesOrderItemRepository;

    public SalesOrderItemService(ISalesOrderItemRepository salesOrderItemRepository)
    {
        _salesOrderItemRepository = salesOrderItemRepository;
    }

    public async Task<SalesOrderItemDto?> GetByIdAsync(int id)
    {
        var item = await _salesOrderItemRepository.GetByIdAsync(id);
        if (item == null) return null;
        return MapToResponseDto(item);
    }

    public async Task<List<SalesOrderItemDto>> GetAllAsync()
    {
        var items = await _salesOrderItemRepository.GetAllAsync();
        return items.Select(MapToResponseDto).ToList();
    }

    public async Task<List<SalesOrderItemDto>> GetBySalesOrderIdAsync(int salesOrderId)
    {
        var items = await _salesOrderItemRepository.GetBySalesOrderIdAsync(salesOrderId);
        return items.Select(MapToResponseDto).ToList();
    }

    public async Task<SalesOrderItemDto> CreateAsync(SalesOrderItemCreateDto dto)
    {
        var item = new SalesOrderItem
        {
            SalesOrderId = dto.SalesOrderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice,
            WarehouseId = dto.WarehouseId
        };

        var createdItem = await _salesOrderItemRepository.CreateAsync(item);
        return MapToResponseDto(createdItem);
    }

    public async Task<SalesOrderItemDto?> UpdateAsync(int id, SalesOrderItemUpdateDto dto)
    {
        var existingItem = await _salesOrderItemRepository.GetByIdAsync(id);
        if (existingItem == null) throw new InvalidOperationException("Sales order item doesn't exist");

        var itemToUpdate = new SalesOrderItem
        {
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice,
            WarehouseId = dto.WarehouseId
        };

        var updatedItem = await _salesOrderItemRepository.UpdateAsync(id, itemToUpdate);
        return updatedItem != null ? MapToResponseDto(updatedItem) : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _salesOrderItemRepository.DeleteAsync(id);
    }

    private SalesOrderItemDto MapToResponseDto(SalesOrderItem item)
    {
        return new SalesOrderItemDto
        {
            Id = item.Id,
            SalesOrderId = item.SalesOrderId,
            ProductId = item.ProductId,
            ProductName = item.Product?.Name,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            WarehouseId = item.WarehouseId,
            WarehouseName = item.Warehouse?.Name
        };
    }
}
