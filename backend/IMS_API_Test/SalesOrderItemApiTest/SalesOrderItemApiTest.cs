// using IMS_API.dtos.SalesOrderItemDTO;
// using IMS_API.Models;
// using IMS_API.Repositories.Interfaces;
// using IMS_API.Services;
// using IMS_API.Services.Interfaces;
// using Moq;
// using Xunit;

// namespace IMS_API_Test.SalesOrderItemApiTest;

// public class SalesOrderItemApiTest
// {
//     private readonly Mock<ISalesOrderItemRepository> _mockRepo;
//     private readonly ISalesOrderItemService _mockService;

//     public SalesOrderItemApiTest()
//     {
//         _mockRepo = new Mock<ISalesOrderItemRepository>();
//         _mockService = new SalesOrderItemService(_mockRepo.Object);
//     }

//     [Fact]
//     public async Task GetByIdAsync_ReturnSalesOrderItem_CheckItemExist()
//     {
//         var expectedItem = new SalesOrderItem
//         {
//             Id = 1,
//             SalesOrderId = 1,
//             ProductId = 1,
//             Quantity = 5,
//             UnitPrice = 100.00m,
//             WarehouseId = 1,
//             Status = "Pending"
//         };
//         _mockRepo.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(expectedItem);

//         var result = await _mockService.GetByIdAsync(1);

//         Assert.NotNull(result);
//         Assert.Equal(1, result.Id);
//         Assert.Equal(5, result.Quantity);
//         Assert.Equal(100.00m, result.UnitPrice);
//     }

//     [Fact]
//     public async Task GetByIdAsync_ReturnNull_WhenItemDoesNotExist()
//     {
//         _mockRepo.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((SalesOrderItem?)null);

//         var result = await _mockService.GetByIdAsync(99);

//         Assert.Null(result);
//     }

//     [Fact]
//     public async Task Create_ReturnSalesOrderItem_IfValidData()
//     {
//         var dto = new SalesOrderItemCreateDto
//         {
//             ProductId = 2,
//             Quantity = 10,
//             UnitPrice = 150.00m,
//             WarehouseId = 1,
//             SalesOrderId = 1,
//             Status = "Shipped"
//         };

//         var createdItem = new SalesOrderItem
//         {
//             Id = 2,
//             ProductId = dto.ProductId,
//             Quantity = dto.Quantity,
//             UnitPrice = dto.UnitPrice,
//             WarehouseId = dto.WarehouseId,
//             SalesOrderId = dto.SalesOrderId,
//             Status = dto.Status
//         };

//         _mockRepo.Setup(r => r.CreateAsync(It.IsAny<SalesOrderItem>())).ReturnsAsync(createdItem);

//         var result = await _mockService.CreateAsync(dto);

//         Assert.NotNull(result);
//         Assert.Equal(10, result.Quantity);
//         Assert.Equal(150.00m, result.UnitPrice);
//         Assert.Equal("Shipped", result.Status);
//     }

//     [Fact]
//     public async Task GetAllAsync_ReturnList_WhenItemsExist()
//     {
//         var items = new List<SalesOrderItem>
//         {
//             new SalesOrderItem { Id = 1, ProductId = 1, Quantity = 5, SalesOrderId = 1 },
//             new SalesOrderItem { Id = 2, ProductId = 2, Quantity = 10, SalesOrderId = 1 }
//         };

//         _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(items);

//         var result = await _mockService.GetAllAsync();

//         Assert.NotNull(result);
//         Assert.Equal(2, result.Count);
//     }

//     [Fact]
//     public async Task GetAllAsync_ReturnEmpty_WhenNoItemsExist()
//     {
//         _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<SalesOrderItem>());

//         var result = await _mockService.GetAllAsync();

//         Assert.NotNull(result);
//         Assert.Empty(result);
//     }

//     [Fact]
//     public async Task UpdateAsync_ReturnUpdatedItem_WhenItemExists()
//     {
//         var dto = new SalesOrderItemUpdateDto
//         {
//             Quantity = 8,
//             UnitPrice = 120.00m,
//             WarehouseId = 2
//         };

//         var existing = new SalesOrderItem
//         {
//             Id = 1,
//             SalesOrderId = 1,
//             ProductId = 1,
//             Quantity = 5,
//             UnitPrice = 100.00m,
//             WarehouseId = 1,
//             Status = "Pending"
//         };

//         _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
//         _mockRepo.Setup(r => r.UpdateAsync(1, It.IsAny<SalesOrderItem>()))
//             .ReturnsAsync(new SalesOrderItem
//             {
//                 Id = 1,
//                 SalesOrderId = 1,
//                 ProductId = 1,
//                 Quantity = dto.Quantity,
//                 UnitPrice = dto.UnitPrice,
//                 WarehouseId = dto.WarehouseId,
//                 Status = "Pending"
//             });

//         var result = await _mockService.UpdateAsync(1, dto);

//         Assert.NotNull(result);
//         Assert.Equal(8, result.Quantity);
//         Assert.Equal(120.00m, result.UnitPrice);
//     }

//     [Fact]
//     public async Task UpdateAsync_ReturnNull_WhenItemDoesNotExist()
//     {
//         _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((SalesOrderItem?)null);

//         var dto = new SalesOrderItemUpdateDto
//         {
//             Quantity = 0,
//             UnitPrice = 0,
//             WarehouseId = 0
//         };

//         var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _mockService.UpdateAsync(99, dto));
//         Assert.Equal("Sales order item doesn't exist", ex.Message);
//     }

//     [Fact]
//     public async Task DeleteAsync_ReturnTrue_WhenItemExists()
//     {
//         _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

//         var result = await _mockService.DeleteAsync(1);

//         Assert.True(result);
//     }

//     [Fact]
//     public async Task DeleteAsync_ReturnFalse_WhenItemDoesNotExist()
//     {
//         _mockRepo.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

//         var result = await _mockService.DeleteAsync(99);

//         Assert.False(result);
//     }
// }
