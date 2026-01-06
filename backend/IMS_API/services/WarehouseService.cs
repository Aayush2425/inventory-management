using IMS_API.dtos.WrehouseDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services.Interfaces;

namespace IMS_API.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IUserRepository _userRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository, IUserRepository userRepository)
    {
        _warehouseRepository = warehouseRepository;
        _userRepository = userRepository;
    }

    public async Task<WarehouseResponseDto?> GetByIdAsync(int id)
    {
        var warehouse = await _warehouseRepository.GetByIdAsync(id);
        if (warehouse == null) return null;

        return MapToResponseDto(warehouse);
    }

    public async Task<List<WarehouseResponseDto>> GetAllAsync()
    {
        var warehouses = await _warehouseRepository.GetAllAsync();
        return warehouses.Select(MapToResponseDto).ToList();
    }

    public async Task<List<WarehouseResponseDto>> GetByUserIdAsync(int userId)
    {
        var warehouses = await _warehouseRepository.GetByUserIdAsync(userId);
        return warehouses.Select(MapToResponseDto).ToList();
    }

    public async Task<WarehouseResponseDto> CreateAsync(WarehouseCreateDto dto)
    {
        var user = await _userRepository.GetByIdAsync(dto.UserId);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        var warehouse = new Warehouse
        {
            Name = dto.Name,
            Location = dto.Location,
            UserId = dto.UserId
        };

        var createdWarehouse = await _warehouseRepository.CreateAsync(warehouse);
        return MapToResponseDto(createdWarehouse);
    }

    public async Task<WarehouseResponseDto?> UpdateAsync(int id, WarehouseUpdateDto dto)
    {
        var existingWarehouse = await _warehouseRepository.GetByIdAsync(id);
        if (existingWarehouse == null) throw new InvalidOperationException("Warehouse doesn't exist");

        var warehouseToUpdate = new Warehouse
        {
            Name = dto.Name,
            Location = dto.Location
        };

        var updatedWarehouse = await _warehouseRepository.UpdateAsync(id, warehouseToUpdate);
        return updatedWarehouse != null ? MapToResponseDto(updatedWarehouse) : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _warehouseRepository.DeleteAsync(id);
    }

    private WarehouseResponseDto MapToResponseDto(Warehouse warehouse)
    {
        return new WarehouseResponseDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Location = warehouse.Location,
            UserId = warehouse.UserId
        };
    }
}
