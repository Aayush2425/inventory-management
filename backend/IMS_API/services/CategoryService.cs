using IMS_API.dtos.CategoryDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services.Interfaces;

namespace IMS_API.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _CategoryRepository;

    public CategoryService(ICategoryRepository CategoryRepository)
    {
        _CategoryRepository = CategoryRepository;
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var Category = await _CategoryRepository.GetByIdAsync(id);
        if (Category == null) return null;

        return MapToResponseDto(Category);
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var Categorys = await _CategoryRepository.GetAllAsync();
        return Categorys.Select(MapToResponseDto).ToList();
    }



    public async Task<CategoryDto> CreateAsync(CategoryCreateDto dto)
    {

        var Category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,

        };

        var createdCategory = await _CategoryRepository.CreateAsync(Category);
        return MapToResponseDto(createdCategory);
    }

    public async Task<CategoryDto?> UpdateAsync(int id, CategoryUpdateDto dto)
    {
        var existingCategory = await _CategoryRepository.GetByIdAsync(id);

        if (existingCategory == null)
            return null; // ✅ matches test expectation

        // ✅ Update the EXISTING entity
        existingCategory.Name = dto.Name;
        existingCategory.Description = dto.Description;

        var updatedCategory = await _CategoryRepository.UpdateAsync(id, existingCategory);

        return updatedCategory != null
            ? MapToResponseDto(updatedCategory)
            : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _CategoryRepository.DeleteAsync(id);
    }

    public CategoryDto MapToResponseDto(Category Category)
    {
        return new CategoryDto
        {
            Id = Category.Id,
            Name = Category.Name,
            Description = Category.Description
        };
    }
}
