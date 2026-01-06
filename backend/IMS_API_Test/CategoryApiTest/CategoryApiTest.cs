using IMS_API.dtos.CategoryDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services;
using IMS_API.Services.Interfaces;
using Moq;
using Xunit;

namespace IMS_API_Test.CategoryApiTest;

public class CategoryApiTest
{
    private readonly Mock<ICategoryRepository> _mockRepo;
    private readonly ICategoryService _mockService;
    public CategoryApiTest()
    {
        _mockRepo = new Mock<ICategoryRepository>();
        _mockService = new CategoryService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnCategory_CheckCategoryExist()
    {
        var expectedCategory = new Category
        {
            Id = 1,
            Name = "Cold Drinks",
            Description = "I don't know"
        };
        _mockRepo.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(expectedCategory);

        var result = await _mockService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Cold Drinks", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnNull_WhenCategoryDoesNotExist()
    {
        var category = new Category
        {
            Id = 1,
            Name = "Cold Drinks",
            Description = "I don't know"
        };
        _mockRepo
            .Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(category);

        var result = await _mockService.GetByIdAsync(99);

        Assert.Null(result);
    }
    [Fact]
    public async Task Create_ReturnCategory_IfNameNotExist()
    {
        var dto = new CategoryCreateDto
        {
            Name = "Electronics",
            Description = "Electronic items"
        };

        var createdCategory = new Category
        {
            Id = 1,
            Name = dto.Name,
            Description = dto.Description
        };


        _mockRepo
            .Setup(r => r.CreateAsync(It.IsAny<Category>()))
            .ReturnsAsync(createdCategory);

        var result = await _mockService.CreateAsync(dto);
        Assert.NotNull(result);
        Assert.Equal("Electronics", result.Name);
    }
    [Fact]
    public async Task GetAllAsync_ReturnList_WhenCategoriesExist()
    {
        var categories = new List<Category>
    {
        new Category { Id = 1, Name = "Cold Drinks" },
        new Category { Id = 2, Name = "Snacks" }
    };

        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

        var result = await _mockService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
    [Fact]
    public async Task GetAllAsync_ReturnEmpty_WhenNoCategoriesExist()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Category>());

        var result = await _mockService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
    [Fact]
    public async Task UpdateAsync_ReturnUpdatedCategory_WhenCategoryExists()
    {
        var dto = new CategoryUpdateDto { Name = "Updated Name", Description = "Updated Desc" };

        var existing = new Category { Id = 1, Name = "Old Name", Description = "Old Desc" };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateAsync(1, existing)).ReturnsAsync(existing);

        var result = await _mockService.UpdateAsync(1, dto);

        Assert.NotNull(result);
        Assert.Equal("Updated Name", result.Name);
    }
    [Fact]
    public async Task UpdateAsync_ReturnNull_WhenCategoryDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Category?)null);

        var dto = new CategoryUpdateDto { Name = "Doesn't Matter", Description = "None" };
        var result = await _mockService.UpdateAsync(99, dto);

        Assert.Null(result);
    }
    [Fact]
    public async Task DeleteAsync_ReturnTrue_WhenCategoryExists()
    {
        var category = new Category { Id = 1, Name = "Test" };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
        _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _mockService.DeleteAsync(1);

        Assert.True(result);
    }
    [Fact]
    public async Task DeleteAsync_ReturnFalse_WhenCategoryDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Category?)null);

        var result = await _mockService.DeleteAsync(99);

        Assert.False(result);
    }


}