using IMS_API.dtos.PurchaseOrderDTO;
using IMS_API.dtos.PurchaseOrderItemDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services.Interfaces;

namespace IMS_API.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;

    public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
    }

    public async Task<PurchaseOrderDto?> GetByIdAsync(int id)
    {
        var order = await _purchaseOrderRepository.GetByIdAsync(id);
        if (order == null) return null;
        return MapToResponseDto(order);
    }

    public async Task<List<PurchaseOrderDto>> GetAllAsync()
    {
        var orders = await _purchaseOrderRepository.GetAllAsync();
        return orders.Select(MapToResponseDto).ToList();
    }

    public async Task<List<PurchaseOrderDto>> GetBySuppliersIdAsync(int supplierId)
    {
        var orders = await _purchaseOrderRepository.GetBySuppliersIdAsync(supplierId);
        return orders.Select(MapToResponseDto).ToList();
    }

    public async Task<List<PurchaseOrderDto>> GetByWarehouseIdAsync(int warehouseId)
    {
        var orders = await _purchaseOrderRepository.GetByWarehouseIdAsync(warehouseId);
        Console.WriteLine($"\n\n\n\n {orders[0].Items}\n\n\n\n");
        return orders.Select(MapToResponseDto).ToList();
    }



    public async Task<PurchaseOrderDto> CreateAsync(PurchaseOrderCreateDto dto)
    {
        var order = new PurchaseOrder
        {
            ExpectedDeliveryDate = dto.ExpectedDeliveryDate,
            OrderDate = dto.OrderDate,
            TotalAmount = dto.TotalAmount,
            OrderNumber = dto.OrderNumber,
            SupplierId = dto.SupplierId,
            IsReceived = dto.IsReceived
        };

        var createdOrder = await _purchaseOrderRepository.CreateAsync(order);
        return MapToResponseDto(createdOrder);
    }

    public async Task<PurchaseOrderDto?> UpdateAsync(int id, PurchaseOrderUpdateDto dto)
    {
        var existingOrder = await _purchaseOrderRepository.GetByIdAsync(id);
        if (existingOrder == null) throw new InvalidOperationException("Purchase order doesn't exist");
        Console.WriteLine($"\n\n\n\n{dto.TotalAmount}\n\n\n\n");
        var orderToUpdate = new PurchaseOrder
        {
            OrderNumber = dto.OrderNumber,
            SupplierId = dto.SupplierId,
            OrderDate = dto.OrderDate,
            ExpectedDeliveryDate = dto.ExpectedDeliveryDate,
            TotalAmount = dto.TotalAmount,
            UpdatedAt = DateTime.Now,
            IsReceived = dto.IsReceived
        };
        Console.WriteLine($"\n\n\n\n{orderToUpdate.TotalAmount}\n\n\n\n");
        var updatedOrder = await _purchaseOrderRepository.UpdateAsync(id, orderToUpdate);
        return updatedOrder != null ? MapToResponseDto(updatedOrder) : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _purchaseOrderRepository.DeleteAsync(id);
    }

    private PurchaseOrderDto MapToResponseDto(PurchaseOrder order)
    {
        return new PurchaseOrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            SupplierId = order.SupplierId,
            SupplierName = order.Supplier?.Name,
            OrderDate = order.OrderDate,
            IsReceived = order.IsReceived,
            ExpectedDeliveryDate = order.ExpectedDeliveryDate,
            TotalAmount = order.TotalAmount,
            Items = order.Items.Select(item => new PurchaseOrderItemDto
            {
                Id = item.Id,
                PurchaseOrderId = item.PurchaseOrderId,
                ProductId = item.ProductId,
                ProductName = item.Product?.Name,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                WarehouseId = item.WarehouseId,
                Status = item.Status
            }).ToList(),
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };
    }
}
