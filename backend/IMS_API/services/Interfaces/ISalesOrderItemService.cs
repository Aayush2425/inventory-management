using IMS_API.dtos.SalesOrderItemDTO;

namespace IMS_API.Services.Interfaces;

public interface ISalesOrderItemService
{
    Task<SalesOrderItemDto?> GetByIdAsync(int id);
    Task<List<SalesOrderItemDto>> GetAllAsync();
    Task<List<SalesOrderItemDto>> GetBySalesOrderIdAsync(int salesOrderId);
    Task<SalesOrderItemDto> CreateAsync(SalesOrderItemCreateDto dto);
    Task<SalesOrderItemDto?> UpdateAsync(int id, SalesOrderItemUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
