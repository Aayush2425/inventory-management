using IMS_API.dtos.SalesOrderDTO;

namespace IMS_API.Services.Interfaces;

public interface ISalesOrderService
{
    Task<SalesOrderDto?> GetByIdAsync(int id);
    Task<List<SalesOrderDto>> GetAllAsync();
    Task<List<SalesOrderDto>> GetByCustomerIdAsync(int customerId);
    Task<SalesOrderDto> CreateAsync(SalesOrderCreateDto dto);
    Task<SalesOrderDto?> UpdateAsync(int id, SalesOrderUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
