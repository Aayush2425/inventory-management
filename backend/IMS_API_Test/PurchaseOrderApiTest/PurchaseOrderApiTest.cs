using IMS_API.dtos.PurchaseOrderDTO;
using IMS_API.dtos.PurchaseOrderItemDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services;
using IMS_API.Services.Interfaces;
using Moq;
using Xunit;

namespace IMS_API_Test.PurchaseOrderApiTest;

public class PurchaseOrderApiTest
{
    private readonly Mock<IPurchaseOrderRepository> _mockRepo;
    private readonly IPurchaseOrderService _mockService;

    public PurchaseOrderApiTest()
    {
        _mockRepo = new Mock<IPurchaseOrderRepository>();
        _mockService = new PurchaseOrderService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnPurchaseOrder_CheckOrderExist()
    {
        var expectedOrder = new PurchaseOrder
        {
            Id = 1,
            OrderNumber = "PO001",
            SupplierId = 1,
            OrderDate = DateTime.UtcNow,
            IsReceived = false,
            TotalAmount = 500.00m
        };
        _mockRepo.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(expectedOrder);

        var result = await _mockService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("PO001", result.OrderNumber);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnNull_WhenOrderDoesNotExist()
    {
        _mockRepo.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((PurchaseOrder?)null);

        var result = await _mockService.GetByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task Create_ReturnPurchaseOrder_IfValidData()
    {
        var dto = new PurchaseOrderCreateDto
        {
            OrderNumber = "PO002",
            SupplierId = 1,
            OrderDate = DateTime.UtcNow,
            ExpectedDeliveryDate = DateTime.UtcNow.AddDays(10),
            IsReceived = false,
            TotalAmount = 750.00m
        };

        var createdOrder = new PurchaseOrder
        {
            Id = 2,
            OrderNumber = dto.OrderNumber,
            SupplierId = dto.SupplierId,
            OrderDate = dto.OrderDate,
            ExpectedDeliveryDate = dto.ExpectedDeliveryDate,
            IsReceived = dto.IsReceived,
            TotalAmount = dto.TotalAmount
        };

        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<PurchaseOrder>())).ReturnsAsync(createdOrder);

        var result = await _mockService.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("PO002", result.OrderNumber);
    }

    [Fact]
    public async Task GetAllAsync_ReturnList_WhenOrdersExist()
    {
        var orders = new List<PurchaseOrder>
        {
            new PurchaseOrder { Id = 1, OrderNumber = "PO001", SupplierId = 1, IsReceived = false },
            new PurchaseOrder { Id = 2, OrderNumber = "PO002", SupplierId = 1, IsReceived = false }
        };

        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

        var result = await _mockService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_ReturnEmpty_WhenNoOrdersExist()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<PurchaseOrder>());

        var result = await _mockService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnUpdatedOrder_WhenOrderExists()
    {
        var dto = new PurchaseOrderUpdateDto
        {
            OrderNumber = "PO001-UPDATED",
            SupplierId = 2,
            IsReceived = true
        };

        var existing = new PurchaseOrder
        {
            Id = 1,
            OrderNumber = "PO001",
            SupplierId = 1,
            IsReceived = false
        };

        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateAsync(1, It.IsAny<PurchaseOrder>()))
            .ReturnsAsync(new PurchaseOrder
            {
                Id = 1,
                OrderNumber = dto.OrderNumber,
                SupplierId = dto.SupplierId,
                IsReceived = dto.IsReceived
            });

        var result = await _mockService.UpdateAsync(1, dto);

        Assert.NotNull(result);
        Assert.Equal("PO001-UPDATED", result.OrderNumber);
    }

    [Fact]
    public async Task UpdateAsync_ReturnNull_WhenOrderDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((PurchaseOrder?)null);

        var dto = new PurchaseOrderUpdateDto
        {
            OrderNumber = "Test",
            SupplierId = 1,
            IsReceived = false
        };

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _mockService.UpdateAsync(99, dto));
        Assert.Equal("Purchase order doesn't exist", ex.Message);
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
