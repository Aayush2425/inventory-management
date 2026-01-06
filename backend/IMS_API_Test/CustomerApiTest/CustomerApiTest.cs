using IMS_API.dtos.CustomerDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services;
using IMS_API.Services.Interfaces;
using Moq;
using Xunit;

namespace IMS_API_Test.CustomerApiTest;

public class CustomerApiTest
{
    private readonly Mock<ICustomerRepository> _mockRepo;
    private readonly ICustomerService _mockService;

    public CustomerApiTest()
    {
        _mockRepo = new Mock<ICustomerRepository>();
        _mockService = new CustomerService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnCustomer_CheckCustomerExist()
    {
        var expectedCustomer = new Customer
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            PhoneNumber = "1234567890",
            Address = "123 Main St",
            WarehouseId = 1
        };
        _mockRepo.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(expectedCustomer);

        var result = await _mockService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("John Doe", result.Name);
        Assert.Equal("john@example.com", result.Email);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnNull_WhenCustomerDoesNotExist()
    {
        _mockRepo.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Customer?)null);

        var result = await _mockService.GetByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task Create_ReturnCustomer_IfValidData()
    {
        var dto = new CustomerCreateDto
        {
            Name = "Jane Smith",
            Email = "jane@example.com",
            PhoneNumber = "0987654321",
            Address = "456 Oak Ave",
            WarehouseId = 1
        };

        var createdCustomer = new Customer
        {
            Id = 2,
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            WarehouseId = dto.WarehouseId
        };

        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Customer>())).ReturnsAsync(createdCustomer);

        var result = await _mockService.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("Jane Smith", result.Name);
        Assert.Equal("jane@example.com", result.Email);
    }

    [Fact]
    public async Task GetAllAsync_ReturnList_WhenCustomersExist()
    {
        var customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "John Doe", Email = "john@example.com", WarehouseId = 1 },
            new Customer { Id = 2, Name = "Jane Smith", Email = "jane@example.com", WarehouseId = 1 }
        };

        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);

        var result = await _mockService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_ReturnEmpty_WhenNoCustomersExist()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Customer>());

        var result = await _mockService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnUpdatedCustomer_WhenCustomerExists()
    {
        var dto = new CustomerUpdateDto
        {
            Name = "John Updated",
            Email = "john.updated@example.com",
            PhoneNumber = "1111111111",
            Address = "789 Pine Rd"
        };

        var existing = new Customer
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            PhoneNumber = "1234567890",
            Address = "123 Main St",
            WarehouseId = 1
        };

        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateAsync(1, It.IsAny<Customer>()))
            .ReturnsAsync(new Customer
            {
                Id = 1,
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                WarehouseId = 1
            });

        var result = await _mockService.UpdateAsync(1, dto);

        Assert.NotNull(result);
        Assert.Equal("John Updated", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_ReturnNull_WhenCustomerDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Customer?)null);

        var dto = new CustomerUpdateDto
        {
            Name = "Doesn't Matter",
            Email = "none@example.com",
            PhoneNumber = "0000000000",
            Address = "None"
        };

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _mockService.UpdateAsync(99, dto));
        Assert.Equal("Customer doesn't exist", ex.Message);
    }

    [Fact]
    public async Task DeleteAsync_ReturnTrue_WhenCustomerExists()
    {
        var customer = new Customer { Id = 1, Name = "Test" };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);
        _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _mockService.DeleteAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnFalse_WhenCustomerDoesNotExist()
    {
        _mockRepo.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

        var result = await _mockService.DeleteAsync(99);

        Assert.False(result);
    }
}
