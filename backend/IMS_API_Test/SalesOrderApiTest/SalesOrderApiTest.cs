using IMS_API.dtos.SalesOrderDTO;
using IMS_API.dtos.SalesOrderItemDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services;
using IMS_API.Services.Interfaces;
using Moq;
using Xunit;

namespace IMS_API_Test.SalesOrderApiTest;

public class SalesOrderApiTest
{
    private readonly Mock<ISalesOrderRepository> _mockRepo;
    private readonly ISalesOrderService _mockService;

    public SalesOrderApiTest()
    {
        _mockRepo = new Mock<ISalesOrderRepository>();
        _mockService = new SalesOrderService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnSalesOrder_CheckOrderExist()
    {
        var expectedOrder = new SalesOrder
        {
            Id = 1,
            OrderNumber = "SO001",
            CustomerId = 1,
            OrderDate = DateTime.UtcNow,
            IsShipped = false,
            Discount = 0,
            Tax = 100.00m,
            TotalAmount = 500.00m
        };
        _mockRepo.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(expectedOrder);

        var result = await _mockService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("SO001", result.OrderNumber);
        Assert.Equal(500.00m, result.TotalAmount);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnNull_WhenOrderDoesNotExist()
    {
        _mockRepo.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((SalesOrder?)null);

        var result = await _mockService.GetByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task Create_ReturnSalesOrder_IfValidData()
    {
        var dto = new SalesOrderCreateDto
        {
            OrderNumber = "SO002",
            CustomerId = 1,
            IsShipped = false,
            Discount = 50.00m,
            Tax = 120.00m,
            TotalAmount = 1000.00m
        };

        var createdOrder = new SalesOrder
        {
            Id = 2,
            OrderNumber = dto.OrderNumber,
            CustomerId = dto.CustomerId,
            OrderDate = DateTime.UtcNow,
            IsShipped = dto.IsShipped,
            Discount = dto.Discount,
            Tax = dto.Tax,
            TotalAmount = dto.TotalAmount
        };

        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<SalesOrder>())).ReturnsAsync(createdOrder);

        var result = await _mockService.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("SO002", result.OrderNumber);
        Assert.Equal(1000.00m, result.TotalAmount);
    }

    [Fact]
    public async Task GetAllAsync_ReturnList_WhenOrdersExist()
    {
        var orders = new List<SalesOrder>
        {
            new SalesOrder { Id = 1, OrderNumber = "SO001", CustomerId = 1, IsShipped = false },
            new SalesOrder { Id = 2, OrderNumber = "SO002", CustomerId = 2, IsShipped = true }
        };

        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

        var result = await _mockService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_ReturnEmpty_WhenNoOrdersExist()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<SalesOrder>());

        var result = await _mockService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnUpdatedOrder_WhenOrderExists()
    {
        var dto = new SalesOrderUpdateDto
        {
            Id = 1,
            OrderNumber = "SO001-UPDATED",
            CustomerId = 2,
            IsShipped = true,
            Discount = 75.00m,
            Tax = 150.00m,
            TotalAmount = 1200.00m
        };

        var existing = new SalesOrder
        {
            Id = 1,
            OrderNumber = "SO001",
            CustomerId = 1,
            IsShipped = false,
            Discount = 50.00m,
            Tax = 100.00m,
            TotalAmount = 1000.00m
        };

        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateAsync(1, It.IsAny<SalesOrder>()))
            .ReturnsAsync(new SalesOrder
            {
                Id = 1,
                OrderNumber = dto.OrderNumber,
                CustomerId = dto.CustomerId,
                IsShipped = dto.IsShipped,
                Discount = dto.Discount,
                Tax = dto.Tax,
                TotalAmount = dto.TotalAmount
            });

        var result = await _mockService.UpdateAsync(1, dto);

        Assert.NotNull(result);
        Assert.Equal("SO001-UPDATED", result.OrderNumber);
        Assert.True(result.IsShipped);
    }

    [Fact]
    public async Task UpdateAsync_ReturnNull_WhenOrderDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((SalesOrder?)null);

        var dto = new SalesOrderUpdateDto
        {
            Id = 99,
            OrderNumber = "Test",
            CustomerId = 1,
            IsShipped = false,
            Discount = 0,
            Tax = 0,
            TotalAmount = 0
        };

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _mockService.UpdateAsync(99, dto));
        Assert.Equal("Sales order doesn't exist", ex.Message);
    }

    [Fact]
    public async Task DeleteAsync_ReturnTrue_WhenOrderExists()
    {
        _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _mockService.DeleteAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnFalse_WhenOrderDoesNotExist()
    {
        _mockRepo.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

        var result = await _mockService.DeleteAsync(99);

        Assert.False(result);
    }
}
