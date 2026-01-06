using IMS_API.dtos.CustomerDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services.Interfaces;

namespace IMS_API.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return null;
        return MapToResponseDto(customer);
    }

    public async Task<List<CustomerDto>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(MapToResponseDto).ToList();
    }

    public async Task<List<CustomerDto>> GetByWarehouseIdAsync(int warehouseId)
    {
        var customers = await _customerRepository.GetByWarehouseIdAsync(warehouseId);
        return customers.Select(MapToResponseDto).ToList();
    }

    public async Task<CustomerDto> CreateAsync(CustomerCreateDto dto)
    {
        var customer = new Customer
        {
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            WarehouseId = dto.WarehouseId
        };

        var createdCustomer = await _customerRepository.CreateAsync(customer);
        return MapToResponseDto(createdCustomer);
    }

    public async Task<CustomerDto?> UpdateAsync(int id, CustomerUpdateDto dto)
    {
        var existingCustomer = await _customerRepository.GetByIdAsync(id);
        if (existingCustomer == null) throw new InvalidOperationException("Customer doesn't exist");

        var customerToUpdate = new Customer
        {
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address
        };

        var updatedCustomer = await _customerRepository.UpdateAsync(id, customerToUpdate);
        return updatedCustomer != null ? MapToResponseDto(updatedCustomer) : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _customerRepository.DeleteAsync(id);
    }

    public async Task<List<CustomerDto>> SearchAsync(int userId, string? query, string? warehouse)
    {
        var customers = await _customerRepository.SearchAsync(userId, query, warehouse);
        return customers.Select(MapToResponseDto).ToList();
    }

    private CustomerDto MapToResponseDto(Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Address = customer.Address,
            WarehouseId = customer.WarehouseId
        };
    }
}
