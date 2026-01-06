using IMS_API.dtos.SupplierDTO;

namespace IMS_API.Services.Interfaces;

public interface ISupplierService
{
    Task<SupplierDto?> GetByIdAsync(int id);
    Task<List<SupplierDto>> GetAllAsync();
    Task<List<SupplierDto>> GetByWarehouseIdAsync(int warehouseId);
    Task<SupplierDto> CreateAsync(SupplierCreateDto dto);
    Task<SupplierDto?> UpdateAsync(int id, SupplierUpdateDto dto);
    Task<bool> DeleteAsync(int id);

    Task<List<SupplierDto>> SearchAsync(int userId, string? query, string? warehouse);
}
