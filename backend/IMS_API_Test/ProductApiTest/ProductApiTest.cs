using IMS_API.dtos.ProductDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services;
using IMS_API.Services.Interfaces;
using Moq;
using Xunit;

namespace IMS_API_Test.ProductApiTest;

public class ProductApiTest
{
    private readonly Mock<IProductRepository> _mockRepo;
    private readonly IProductService _mockService;

    public ProductApiTest()
    {
        _mockRepo = new Mock<IProductRepository>();
        _mockService = new ProductService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnProduct_CheckProductExist()
    {
        var expectedProduct = new Product
        {
            Id = 1,
            Name = "Laptop",
            CategoryId = 1,
            Description = "High-performance laptop",
            Price = 999.99m,
            SKU = "LAPTOP001",
            IsActive = true,
            UserId = 1
        };
        _mockRepo.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(expectedProduct);

        var result = await _mockService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Laptop", result.Name);
        Assert.Equal("LAPTOP001", result.SKU);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnNull_WhenProductDoesNotExist()
    {
        _mockRepo.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Product?)null);

        var result = await _mockService.GetByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task Create_ReturnProduct_IfValidData()
    {
        var dto = new ProductCreateDto
        {
            Name = "Monitor",
            CategoryId = 2,
            Description = "4K Monitor",
            Price = 399.99m,
            SKU = "MON001",
            IsActive = true,
            UserId = 1
        };

        var createdProduct = new Product
        {
            Id = 2,
            Name = dto.Name,
            CategoryId = dto.CategoryId,
            Description = dto.Description,
            Price = dto.Price,
            SKU = dto.SKU,
            IsActive = dto.IsActive,
            UserId = dto.UserId
        };

        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Product>())).ReturnsAsync(createdProduct);

        var result = await _mockService.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("Monitor", result.Name);
        Assert.Equal("MON001", result.SKU);
    }

    [Fact]
    public async Task GetAllAsync_ReturnList_WhenProductsExist()
    {
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", SKU = "LAPTOP001", IsActive = true },
            new Product { Id = 2, Name = "Monitor", SKU = "MON001", IsActive = true }
        };

        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

        var result = await _mockService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_ReturnEmpty_WhenNoProductsExist()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Product>());

        var result = await _mockService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnUpdatedProduct_WhenProductExists()
    {
        var dto = new ProductUpdateDto
        {
            Id = 1,
            Name = "Gaming Laptop",
            CategoryId = 1,
            Description = "High-performance gaming laptop",
            Price = 1299.99m,
            SKU = "LAPTOP001",
            IsActive = true,
            UserId = 1
        };

        var existing = new Product
        {
            Id = 1,
            Name = "Laptop",
            SKU = "LAPTOP001",
            IsActive = true
        };

        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateAsync(1, It.IsAny<Product>()))
            .ReturnsAsync(new Product
            {
                Id = 1,
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                Description = dto.Description,
                Price = dto.Price,
                SKU = dto.SKU,
                IsActive = dto.IsActive,
                UserId = dto.UserId
            });

        var result = await _mockService.UpdateAsync(1, dto);

        Assert.NotNull(result);
        Assert.Equal("Gaming Laptop", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_ReturnNull_WhenProductDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Product?)null);

        var dto = new ProductUpdateDto
        {
            Id = 99,
            Name = "Doesn't Matter",
            CategoryId = 1,
            Description = "None",
            Price = 0,
            SKU = "N/A",
            IsActive = false,
            UserId = 1
        };

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _mockService.UpdateAsync(99, dto));
        Assert.Equal("Product doesn't exist", ex.Message);
    }

    [Fact]
    public async Task DeleteAsync_ReturnTrue_WhenProductExists()
    {
        var product = new Product { Id = 1, Name = "Test Product" };
        _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _mockService.DeleteAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnFalse_WhenProductDoesNotExist()
    {
        _mockRepo.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

        var result = await _mockService.DeleteAsync(99);

        Assert.False(result);
    }
}
