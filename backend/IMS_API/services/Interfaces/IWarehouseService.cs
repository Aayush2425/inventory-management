using IMS_API.dtos.WrehouseDTO;

namespace IMS_API.Services.Interfaces;

public interface IWarehouseService
{
    Task<WarehouseResponseDto?> GetByIdAsync(int id);
    Task<List<WarehouseResponseDto>> GetAllAsync();
    Task<List<WarehouseResponseDto>> GetByUserIdAsync(int userId);
    Task<WarehouseResponseDto> CreateAsync(WarehouseCreateDto dto);
    Task<WarehouseResponseDto?> UpdateAsync(int id, WarehouseUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
