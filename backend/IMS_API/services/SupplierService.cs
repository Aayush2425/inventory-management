using IMS_API.dtos.SupplierDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services.Interfaces;

namespace IMS_API.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;

    public SupplierService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<SupplierDto?> GetByIdAsync(int id)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);
        if (supplier == null) return null;
        return MapToResponseDto(supplier);
    }

    public async Task<List<SupplierDto>> GetAllAsync()
    {
        var suppliers = await _supplierRepository.GetAllAsync();
        return suppliers.Select(MapToResponseDto).ToList();
    }
    public async Task<List<SupplierDto>> GetByWarehouseIdAsync(int warehouseId)
    {
        var suppliers = await _supplierRepository.GetByWarehouseIdAsync(warehouseId);
        return suppliers.Select(MapToResponseDto).ToList();
    }

    public async Task<SupplierDto> CreateAsync(SupplierCreateDto dto)
    {

        var supplier = new Supplier
        {
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            WarehouseId = dto.WarehouseId
        };

        var createdSupplier = await _supplierRepository.CreateAsync(supplier);
        return MapToResponseDto(createdSupplier);
    }

    public async Task<SupplierDto?> UpdateAsync(int id, SupplierUpdateDto dto)
    {
        var existingSupplier = await _supplierRepository.GetByIdAsync(id);
        if (existingSupplier == null) throw new InvalidOperationException("Supplier doesn't exist");

        var supplierToUpdate = new Supplier
        {
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address
        };

        var updatedSupplier = await _supplierRepository.UpdateAsync(id, supplierToUpdate);
        return updatedSupplier != null ? MapToResponseDto(updatedSupplier) : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _supplierRepository.DeleteAsync(id);
    }

    public async Task<List<SupplierDto>> SearchAsync(int userId, string? query, string? warehouse)
    {
        var suppliers = await _supplierRepository.SearchAsync(userId, query, warehouse);
        return suppliers.Select(MapToResponseDto).ToList();
    }

    private SupplierDto MapToResponseDto(Supplier supplier)
    {
        return new SupplierDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Email = supplier.Email,
            PhoneNumber = supplier.PhoneNumber,
            Address = supplier.Address,
            WarehouseId = supplier.WarehouseId
        };
    }
}
