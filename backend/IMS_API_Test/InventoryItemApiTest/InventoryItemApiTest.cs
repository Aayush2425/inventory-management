using IMS_API.dtos.InventoryItemDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services;
using IMS_API.Services.Interfaces;
using Moq;
using Xunit;

namespace IMS_API_Test.InventoryItemApiTest;

public class InventoryItemApiTest
{
    private readonly Mock<IInventoryItemRepository> _mockRepo;
    private readonly IInventoryItemService _mockService;

    public InventoryItemApiTest()
    {
        _mockRepo = new Mock<IInventoryItemRepository>();
        _mockService = new InventoryItemService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnInventoryItem_CheckItemExist()
    {
        var expectedItem = new InventoryItem
        {
            Id = 1,
            ProductId = 1,
            WarehouseId = 1,
            Quantity = 100,
            ReorderLevel = 20,
            Reserved = 10
        };
        _mockRepo.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(expectedItem);

        var result = await _mockService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(100, result.Quantity);
        Assert.Equal(20, result.ReorderLevel);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnNull_WhenItemDoesNotExist()
    {
        _mockRepo.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((InventoryItem?)null);

        var result = await _mockService.GetByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task Create_ReturnInventoryItem_IfValidData()
    {
        var dto = new InventoryItemCreateDto
        {
            ProductId = 2,
            WarehouseId = 1,
            Quantity = 50,
            ReorderLevel = 10,
            Reserved = 5
        };

        var createdItem = new InventoryItem
        {
            Id = 2,
            ProductId = dto.ProductId,
            WarehouseId = dto.WarehouseId,
            Quantity = dto.Quantity,
            ReorderLevel = dto.ReorderLevel,
            Reserved = dto.Reserved
        };

        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<InventoryItem>())).ReturnsAsync(createdItem);

        var result = await _mockService.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal(50, result.Quantity);
        Assert.Equal(2, result.ProductId);
    }

    [Fact]
    public async Task GetAllAsync_ReturnList_WhenItemsExist()
    {
        var items = new List<InventoryItem>
        {
            new InventoryItem { Id = 1, ProductId = 1, WarehouseId = 1, Quantity = 100 },
            new InventoryItem { Id = 2, ProductId = 2, WarehouseId = 1, Quantity = 50 }
        };

        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(items);

        var result = await _mockService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_ReturnEmpty_WhenNoItemsExist()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<InventoryItem>());

        var result = await _mockService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnUpdatedItem_WhenItemExists()
    {
        var dto = new InventoryItemUpdateDto
        {
            Quantity = 150,
            ReorderLevel = 30,
            Reserved = 20
        };

        var existing = new InventoryItem
        {
            Id = 1,
            ProductId = 1,
            WarehouseId = 1,
            Quantity = 100,
            ReorderLevel = 20,
            Reserved = 10
        };

        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateAsync(1, It.IsAny<InventoryItem>()))
            .ReturnsAsync(new InventoryItem
            {
                Id = 1,
                ProductId = 1,
                WarehouseId = 1,
                Quantity = dto.Quantity,
                ReorderLevel = dto.ReorderLevel,
                Reserved = dto.Reserved
            });

        var result = await _mockService.UpdateAsync(1, dto);

        Assert.NotNull(result);
        Assert.Equal(150, result.Quantity);
    }

    [Fact]
    public async Task UpdateAsync_ReturnNull_WhenItemDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((InventoryItem?)null);

        var dto = new InventoryItemUpdateDto
        {
            Quantity = 0,
            ReorderLevel = 0,
            Reserved = 0
        };

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _mockService.UpdateAsync(99, dto));
        Assert.Equal("Inventory item doesn't exist", ex.Message);
    }

    [Fact]
    public async Task DeleteAsync_ReturnTrue_WhenItemExists()
    {
        var item = new InventoryItem { Id = 1 };
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
