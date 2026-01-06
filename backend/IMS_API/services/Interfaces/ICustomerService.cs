using IMS_API.dtos.CustomerDTO;

namespace IMS_API.Services.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto?> GetByIdAsync(int id);
    Task<List<CustomerDto>> GetAllAsync();
    Task<List<CustomerDto>> GetByWarehouseIdAsync(int warehouseId);
    Task<CustomerDto> CreateAsync(CustomerCreateDto dto);

    Task<CustomerDto?> UpdateAsync(int id, CustomerUpdateDto dto);
    Task<bool> DeleteAsync(int id);

    Task<List<CustomerDto>> SearchAsync(int userId, string? query, string? warehouse);
}
