using IMS_API.dtos.PurchaseOrderItemDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services;
using IMS_API.Services.Interfaces;
using Moq;
using Xunit;

namespace IMS_API_Test.PurchaseOrderItemApiTest;

public class PurchaseOrderItemApiTest
{
    private readonly Mock<IPurchaseOrderItemRepository> _mockRepo;
    private readonly IPurchaseOrderItemService _mockService;

    public PurchaseOrderItemApiTest()
    {
        _mockRepo = new Mock<IPurchaseOrderItemRepository>();
        _mockService = new PurchaseOrderItemService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnPurchaseOrderItem_CheckItemExist()
    {
        var expectedItem = new PurchaseOrderItem
        {
            Id = 1,
            PurchaseOrderId = 1,
            ProductId = 1,
            Quantity = 10,
            UnitPrice = 50.00m,
            WarehouseId = 1,
            Status = "Pending"
        };
        _mockRepo.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(expectedItem);

        var result = await _mockService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(10, result.Quantity);
        Assert.Equal("Pending", result.Status);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnNull_WhenItemDoesNotExist()
    {
        _mockRepo.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((PurchaseOrderItem?)null);

        var result = await _mockService.GetByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task Create_ReturnPurchaseOrderItem_IfValidData()
    {
        var dto = new PurchaseOrderItemCreateDto
        {
            ProductId = 2,
            Quantity = 20,
            UnitPrice = 75.00m,
            WarehouseId = 1,
            PurchaseOrderId = 1,
            Status = "Pending"
        };

        var createdItem = new PurchaseOrderItem
        {
            Id = 2,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice,
            WarehouseId = dto.WarehouseId,
            PurchaseOrderId = dto.PurchaseOrderId,
            Status = dto.Status
        };

        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<PurchaseOrderItem>())).ReturnsAsync(createdItem);

        var result = await _mockService.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal(20, result.Quantity);
        Assert.Equal(75.00m, result.UnitPrice);
    }

    [Fact]
    public async Task GetAllAsync_ReturnList_WhenItemsExist()
    {
        var items = new List<PurchaseOrderItem>
        {
            new PurchaseOrderItem { Id = 1, ProductId = 1, Quantity = 10, PurchaseOrderId = 1 },
            new PurchaseOrderItem { Id = 2, ProductId = 2, Quantity = 20, PurchaseOrderId = 1 }
        };

        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(items);

        var result = await _mockService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_ReturnEmpty_WhenNoItemsExist()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<PurchaseOrderItem>());

        var result = await _mockService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnUpdatedItem_WhenItemExists()
    {
        var dto = new PurchaseOrderItemUpdateDto
        {
            Quantity = 15,
            UnitPrice = 55.00m,
            WarehouseId = 2
        };

        var existing = new PurchaseOrderItem
        {
            Id = 1,
            PurchaseOrderId = 1,
            ProductId = 1,
            Quantity = 10,
            UnitPrice = 50.00m,
            WarehouseId = 1,
            Status = "Pending"
        };

        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateAsync(1, It.IsAny<PurchaseOrderItem>()))
            .ReturnsAsync(new PurchaseOrderItem
            {
                Id = 1,
                PurchaseOrderId = 1,
                ProductId = 1,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                WarehouseId = dto.WarehouseId,
                Status = "Pending"
            });

        var result = await _mockService.UpdateAsync(1, dto);

        Assert.NotNull(result);
        Assert.Equal(15, result.Quantity);
    }

    [Fact]
    public async Task UpdateAsync_ReturnNull_WhenItemDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((PurchaseOrderItem?)null);

        var dto = new PurchaseOrderItemUpdateDto
        {
            Quantity = 0,
            UnitPrice = 0,
            WarehouseId = 0
        };

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _mockService.UpdateAsync(99, dto));
        Assert.Equal("Purchase order item doesn't exist", ex.Message);
    }

    [Fact]
    public async Task DeleteAsync_ReturnTrue_WhenItemExists()
    {
        _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _mockService.DeleteAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnFalse_WhenItemDoesNotExist()
    {
        _mockRepo.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

        var result = await _mockService.DeleteAsync(99);

        Assert.False(result);
    }
}
