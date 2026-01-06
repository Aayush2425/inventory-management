using IMS_API.dtos.SalesOrderDTO;
using IMS_API.dtos.SalesOrderItemDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services.Interfaces;

namespace IMS_API.Services;

public class SalesOrderService : ISalesOrderService
{
    private readonly ISalesOrderRepository _salesOrderRepository;

    public SalesOrderService(ISalesOrderRepository salesOrderRepository)
    {
        _salesOrderRepository = salesOrderRepository;
    }

    public async Task<SalesOrderDto?> GetByIdAsync(int id)
    {
        var order = await _salesOrderRepository.GetByIdAsync(id);
        if (order == null) return null;
        return MapToResponseDto(order);
    }

    public async Task<List<SalesOrderDto>> GetAllAsync()
    {
        var orders = await _salesOrderRepository.GetAllAsync();
        return orders.Select(MapToResponseDto).ToList();
    }

    public async Task<List<SalesOrderDto>> GetByCustomerIdAsync(int customerId)
    {
        var orders = await _salesOrderRepository.GetByCustomerIdAsync(customerId);
        return orders.Select(MapToResponseDto).ToList();
    }

    public async Task<SalesOrderDto> CreateAsync(SalesOrderCreateDto dto)
    {
        var order = new SalesOrder
        {
            OrderNumber = dto.OrderNumber,
            CustomerId = dto.CustomerId,
            IsShipped = dto.IsShipped,
            Discount = dto.Discount,
            Tax = dto.Tax,
            TotalAmount = dto.TotalAmount
        };

        var createdOrder = await _salesOrderRepository.CreateAsync(order);
        return MapToResponseDto(createdOrder);
    }

    public async Task<SalesOrderDto?> UpdateAsync(int id, SalesOrderUpdateDto dto)
    {
        var existingOrder = await _salesOrderRepository.GetByIdAsync(id);
        if (existingOrder == null) throw new InvalidOperationException("Sales order doesn't exist");

        var orderToUpdate = new SalesOrder
        {
            OrderNumber = dto.OrderNumber,
            CustomerId = dto.CustomerId,
            IsShipped = dto.IsShipped,
            Discount = dto.Discount,
            Tax = dto.Tax,
            TotalAmount = dto.TotalAmount
        };

        var updatedOrder = await _salesOrderRepository.UpdateAsync(id, orderToUpdate);
        return updatedOrder != null ? MapToResponseDto(updatedOrder) : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _salesOrderRepository.DeleteAsync(id);
    }

    private SalesOrderDto MapToResponseDto(SalesOrder order)
    {
        return new SalesOrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            OrderDate = order.OrderDate,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer?.Name,
            IsShipped = order.IsShipped,
            Discount = order.Discount,
            Tax = order.Tax,
            TotalAmount = order.TotalAmount,
            Items = order.Items.Select(item => new SalesOrderItemDto
            {
                Id = item.Id,
                SalesOrderId = item.SalesOrderId,
                ProductId = item.ProductId,
                ProductName = item.Product?.Name,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                WarehouseId = item.WarehouseId,
            }).ToList(),
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };
    }
}

