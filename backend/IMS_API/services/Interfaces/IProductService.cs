using IMS_API.dtos.ProductDTO;

namespace IMS_API.Services.Interfaces;

public interface IProductService
{
    Task<ProductDto?> GetByIdAsync(int id);
    Task<List<ProductDto>> GetAllAsync();
    Task<List<ProductDto>> GetByUserIdAsync(int userId);

    Task<List<ProductDto>> GetByCategoryIdAsync(int categoryId);
    Task<ProductDto> CreateAsync(ProductCreateDto dto);
    Task<ProductDto?> UpdateAsync(int id, ProductUpdateDto dto);
    Task<List<ProductDto>> SearchAsync(int userId, string? query, string? category);
    Task<bool> DeleteAsync(int id);
}