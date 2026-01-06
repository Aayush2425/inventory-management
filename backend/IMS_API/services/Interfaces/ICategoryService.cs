using IMS_API.dtos.CategoryDTO;
using IMS_API.Models;

namespace IMS_API.Services.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryDto> CreateAsync(CategoryCreateDto dto);
    Task<CategoryDto?> UpdateAsync(int id, CategoryUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
